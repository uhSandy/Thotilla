using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using TutorialWebApplication.Models;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace TutorialWebApplication.Controllers
{
    public class SoundController : ApiController
    {
        enum MobileResponse { stopSwing, playSong, playVideo };

        /*Cry detect post method start*/
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Sound/CryDetect")]
        [ValidateAntiForgeryToken]
        public async Task<String> CryDetectAsync([Bind(Include = "Id,GuId,DateTime,Status")] PlaySong sound)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<PlaySong>.CryDetectAsync(sound);
                return "Cry detect valid";
                //push notification
            }

            //push notification
            return "Cry detect invalid";
        }
        /*Cry detect post method end*/

        /*Mobile POST respponse for baby cry start*/
            [System.Web.Http.HttpPost]
            [System.Web.Http.Route("api/Sound/MobileResponse")]
            [ValidateAntiForgeryToken]
            public async Task<String> MobileResponseAsync([Bind(Include = "Id,GuId,DateTime,Response,ResponseDone")] Sound sound)
            {
                if (ModelState.IsValid)
                {
                    await DocumentDBRepository<PlaySong>.CryMobileResAsync(sound);
                    return "Response Gone Success";
                }

                return "Response Gone Fail";
           }
        /*Mobile POST respponse for baby cry end*/

        /*Get mobile response start*/
        
        [System.Web.Http.HttpGet]
        public async Task<PlaySong> SoundAsync(string id)
        {
            //await DocumentDBRepository<Sound>.GetSoundAsync(id.ToString());

            PlaySong sound = DocumentDBRepository<PlaySong>.mobileNotifyBabyCry(id);

            if (sound!=null) {
                //   return "Baby's Cry Detect for id:"+sound.Id;
                return sound;
            }

            /* Sound sound = new Models.Sound();
             sound.DateTime = "01/03/2018 12.25PM";
             sound.Response = "Baby Crying";
             return sound;*/
            return null;
        }
        /*Get mobile response end*/

        /*update response worked start*/
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Sound/UpdateResponseDoneAsync")]
        [ValidateAntiForgeryToken]
        public async Task<String> UpdateResponseDoneAsync([Bind(Include = "Id,Status")] Sound sound)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Sound>.UpdateMobileResAsync(sound);
                return "Response Update Success";
            }

            return "Response Update Fail";
        }
        /*update response worked end*/

        //[System.Web.Http.HttpGet]
        // [System.Web.Mvc.Route("GetMobileResponseAsync")]//[Route("/someotherapi/[controller]/GetByMemberId")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("usergroups")]
        public async Task<string> GetUserGroups()
        {
            int id = 9;
            Sound sound = await DocumentDBRepository<Sound>.GetSoundAsync(id.ToString());
            return sound.Response;

        }
        /*Get mobile response end*/


        /*cry detect mobile response det method start*/
        /*  [System.Web.Http.HttpGet]
          [System.Web.Http.ActionName("api/Sound/MobileResponse")]
          public async Task<String> DetailsAsync(string id)
          {
              Sound sound = await DocumentDBRepository<Sound>.GetResponseAsync(id);
              return sound.GuId;
          }*/
        /*cry detect mobile response det method end*/

        /* GET: Sound
        public ActionResult Index()
        {
            return View();
        }*/
    }
}