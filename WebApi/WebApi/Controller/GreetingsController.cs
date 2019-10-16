using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Model;

namespace WebApi.Controller {
    [RoutePrefix("api/Greetings")]
    public class GreetingsController : ApiController {
        private static GreetingsRepository Repository = new GreetingsRepository();
        
        [HttpGet]
        [Route("")]
        public IQueryable<Greeting> Get() {
            return Repository.AsQuerable();
        }
        
        [HttpGet]
        [Route("{key:long}")]
        public async Task<IHttpActionResult> Get(long key) {
            var greeting = Repository.Get(key);
            if (greeting == null) {
                return NotFound();
            }
            return Ok(SingleResult.Create(new[] { greeting }.AsQueryable()));
        }

        [HttpPost]
        [Route("LongOnes")]
        public async Task<IHttpActionResult> LongOnes([FromBody]LongOnesParameter param) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var messages = new List<string>();
            foreach (Greeting greeting in Repository.All) {
                if (greeting.Text.Length >= param.Length) {
                    messages.Add(greeting.Text);
                }
            }
            return await Task.FromResult(Ok(messages));
        }
    }

    public class LongOnesParameter {
        public int Length { get; set; }
    }
}
