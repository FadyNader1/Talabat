using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Errors;

namespace Talabat.Controllers
{
    
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class Errorcontroller : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return NotFound(new ApiHandleError(404));
        }
    }
}
