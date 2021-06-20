using Application.Common.Attributes;
using Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Base
{
    [ApiKeyAuthentication]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeyController : ControllerBase
    {
        protected IActionResult ReturnResult(Result result)
        {
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        protected IActionResult ReturnResult<T>(Result<T> result)
        {
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }
    }
}