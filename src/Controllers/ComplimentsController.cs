using System;
using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

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
            try
            {
                var result = await _caseService.CreateComplimentCase(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController CreateCase an exception has occured while calling compliments complaints createCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }

        [Route("submit-compliment-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody]PostData data)
        {
            try
            {
                var result = await _caseService.CreateComplimentCaseFormBuilder(data);

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
