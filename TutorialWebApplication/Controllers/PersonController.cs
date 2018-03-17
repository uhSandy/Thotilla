using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using TutorialWebApplication.Models;
using System.Web.Mvc;

namespace TutorialWebApplication.Controllers
{
    public class PersonController : ApiController
    {

        /*Cry detect post method start*/
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("api/Person/CryDetect")]
        [ValidateAntiForgeryToken]
        public async Task<String> CryDetectAsync([Bind(Include = "Id,GuId,DateTime,Status")] PlaySong sound)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<PlaySong>.CreateItemAsync(sound);
                return "Cry detect valid person";
            }

            return "Cry detect invalid person";
        }
        /*Cry detect post method end*/

        /*
                [System.Web.Http.HttpPost]
                [System.Web.Http.ActionName("api/Person/Create")]
                [ValidateAntiForgeryToken]
                public async Task<String> CreateAsync([Bind(Include = "Id,Code,Name")] Person person)
                {
                    if (ModelState.IsValid)
                    {
                        await DocumentDBRepository<Person>.CreateItemAsync(person);
                        return"Index";
                    }

                    return "test";
                }
        */
        /*    [System.Web.Http.HttpGet]
             [System.Web.Http.ActionName("api/Person/Details")]
             public async Task<string> DetailsAsync(int id)
             {
               Person person = await DocumentDBRepository<Person>.GetItemAsync(id.ToString());

               return person.Name;
             }*/

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("api/Person/Details")]
        public async Task<String> DetailsAsync(string id)
        {

            Sound sound = DocumentDBRepository<PlaySong>.mobileResponseForBabyCry(id);

            if (sound != null)
            {
                return sound.Response;
            }
            
            return null;
        }

        [System.Web.Http.HttpGet]
        public async Task<string> ResDetailsAsync(int id,int t)
        {
            Person person = await DocumentDBRepository<Person>.GetItemAsync(id.ToString());
            return person.Name;
        }

        /*        [System.Web.Http.ActionName("Details")]
                public async Task<string> DetailsAsync(int id)
                {
                    Person item = await DocumentDBRepository<Person>.GetItemAsync(id.ToString());
                    return "test";
                }
        */
        // GET: api/Person/5
        /*    public async Task<String> Get(int id)
            {
                Person person = await DocumentDBRepository<Person>.GetItemAsync(id.ToString());
                return "some person don know " + person.Name;
            }
           */

        /*   // GET: api/Person
           public IEnumerable<string> Get()
           {
               return new string[] { "person1", "person2" };
           }
           */

        // POST: api/Person
        [System.Web.Http.Route("api/Person/FullDetails")]
        [System.Web.Http.HttpPost]
        public string Post([FromBody]string value)
        {

            return "blah";
        }

        // PUT: api/Person/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
        }


    }//end of class
}
