using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Threading.Tasks;
using TutorialWebApplication.Models;
using System.Web.Http.Cors;

namespace TutorialWebApplication.Controllers
{
    public class PlaySongController : ApiController
    {
        // GET: PlaySong
       /* public ActionResult Index()
        {
            return View();
        }*/

        [System.Web.Http.HttpGet]
        public async Task<String> PlaySongAsync(string id)
        {
            //await DocumentDBRepository<Sound>.GetSoundAsync(id.ToString());
            PlaySong playSong = DocumentDBRepository<PlaySong>.playSongResponse(id);

            if (playSong != null)
            {
                return playSong.Song;
            }

            return null;
        }

        /*Song play post method start*/
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/PlaySong/SelectedSong")]
        [ValidateAntiForgeryToken]
        public async Task<String> SelectedSongAsync([Bind(Include = "Id,GuId,DateTime,Status,Song")] PlaySong playSong)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<PlaySong>.PlaySongSendAsync(playSong);
                return "Song play valid";
            }
            
            return "Song play invalid";
        }
        /*Song play post method end*/
    }
}