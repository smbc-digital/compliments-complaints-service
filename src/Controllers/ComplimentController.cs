using System;
using System.Threading.Tasks;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Compliment")]
    [ApiController]
    [TokenAuthentication]
    public class ComplimentController : ControllerBase
    {
        private readonly ICaseService _caseService;
        private readonly ILogger<ComplimentController> _logger;

        public ComplimentController(ICaseService caseService, ILogger<ComplimentController> logger)
        {
            _caseService = caseService;
            _logger = logger;
        }

        [Route("submit-compliment")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]ComplimentDetails model)
        {
            try
            {
                var result = await _caseService.CreateComplimentCase(model);
                //await _caseService.CreateComplimentCase(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController GetCase an exception has occured while calling compliments complaints getCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }
    }
}
