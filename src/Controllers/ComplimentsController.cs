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
    [Route("api/v1/Compliments")]
    [ApiController]
    [TokenAuthentication]
    public class ComplimentsController : ControllerBase
    {
        private readonly IComplimentsService _caseService;
        private readonly ILogger<ComplimentsController> _logger;

        public ComplimentsController(IComplimentsService caseService, ILogger<ComplimentsController> logger)
        {
            _caseService = caseService;
            _logger = logger;
        }

        [Route("submit-compliment")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]ComplimentDetails model)
        {
            _logger.LogWarning("******starting create case");
            try
            {
                var result = await _caseService.CreateComplimentCase(model);
                //await _caseService.CreateComplimentCase(model);

                _logger.LogWarning($"******result of create case OK{result}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController CreateCase an exception has occured while calling compliments complaints createCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }
    }
}
