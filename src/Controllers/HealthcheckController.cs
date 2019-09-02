using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace compliments_complaints_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class HealthcheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var assembly = Assembly.GetEntryAssembly().GetName();
            return Ok($"{{'Verson': '{assembly.Version.ToString()}', 'Name': '{assembly.Name}'}}");
        }
    }
}