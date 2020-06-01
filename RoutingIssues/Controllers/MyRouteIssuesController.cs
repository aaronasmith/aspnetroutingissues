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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetMyItem(string param1, int param2) {
            return Ok(1);
        }

        [HttpGet("MyRelatedItems/{param1:int}"), MapToApiVersion("2"), MapToApiVersion("3")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<int>>> GetMyRelatedItems(int param1) {
            return Ok(new[] { 1, 2, 3 });
        }
    }
}