using Application.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Health
{
    public class HealthController : ApiKeyController
    {
        [AllowAno
        [HttpGet]
        public ActionResult Get() => Ok();
    }
}