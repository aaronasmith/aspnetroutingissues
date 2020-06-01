using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutingIssues.Controllers {
    [ApiVersion("2", Deprecated = true)]
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class MyRouteIssuesController : ControllerBase {
        [HttpGet("{param1:regex(^[[0-9]]*$)}/{param2:int}", Order = 1), MapToApiVersion("3")]
        public ActionResult<string> GetMyItem(string param1, int param2) {
            return Ok(param1 + param2);
        }

        [HttpGet("MyRelatedItems/{param1:int}")]
        public ActionResult<IEnumerable<int>> GetMyRelatedItems(int param1) {
            return Ok(new[] { param1 });
        }
    }
}