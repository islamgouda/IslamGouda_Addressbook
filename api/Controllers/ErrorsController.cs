using api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    public class ErrorsController : BaseApiController
    {
      /* public IActionResult Error(int code) {
            return new ObjectResult(new ApiResponse(code));
        }*/
    }
}
