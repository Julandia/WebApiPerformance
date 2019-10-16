using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using OData.Model;

namespace OData.Controller {
    public class GreetingsController : ODataController {
        private static GreetingsRepository Repository = new GreetingsRepository();

        [EnableQuery]
        //[HttpGet]
        public IQueryable<Greeting> Get() {
            return Repository.AsQuerable();
        }

        [EnableQuery]
        //[HttpGet]
        public async Task<IHttpActionResult> Get(long key) {
            var greeting = Repository.Get(key);
            if (greeting == null) {
                return NotFound();
            }
            return Ok(SingleResult.Create(new[] { greeting }.AsQueryable()));
        }

        public async Task<IHttpActionResult> Post(Greeting greeting) {
            Repository.Add(greeting);
            return Ok(greeting);
        }

        [HttpPost]
        public async Task<IHttpActionResult> LongOnes(ODataActionParameters parameters) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            int length = (int)parameters["length"];
            var messages = new List<string>();
            foreach (Greeting greeting in Repository.All) {
                if (greeting.Text.Length >= length) {
                    messages.Add(greeting.Text);
                }
            }
            return await Task.FromResult(Ok(messages));
        }
    }
}
