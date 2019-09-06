using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using compliments_complaints_service.Controllers.HealthCheck.Models;

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
            var name = Assembly.GetEntryAssembly()?.GetName().Name;
            var assembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "compliments-complaints-service.dll");
            var version = FileVersionInfo.GetVersionInfo(assembly).FileVersion;

            return Ok(new HealthCheckModel
            {
                AppVersion = version,
                Name = name
            });
        }
    }
}