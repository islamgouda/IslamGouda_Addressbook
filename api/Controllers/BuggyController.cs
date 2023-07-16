using api.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly Context _context;
             
        public BuggyController(Context context)
        {
            _context = context;
        }
        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            var thing = _context.Jobs.Find(50);
            if (thing == null) { return NotFound(new ApiResponse(404)); }
            return Ok();

        }
        [HttpGet("ServerError")]
        public IActionResult GetServerError()
        {
            var thing=_context.Jobs.Find(50);
            var thingToReturn = thing.ToString();
            return Ok();
        }
        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
           // var thing = _context.Products.Find(50);
            //var thingToReturn = thing.ToString();
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public IActionResult GetNotFoundRequest(int id)
        {
            // var thing = _context.Products.Find(50);
            //var thingToReturn = thing.ToString();
            return Ok();
        }

    }
}
