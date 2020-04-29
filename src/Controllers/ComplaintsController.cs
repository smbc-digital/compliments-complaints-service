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
    [Route("api/v1/Complaints")]
    [ApiController]
    [TokenAuthentication]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintsService _caseService;
        private readonly ILogger<ComplaintsController> _logger;

        public ComplaintsController(ILogger<ComplaintsController> logger, IComplaintsService caseService)
        {
            _caseService = caseService;
            _logger = logger;
        }

        [Route("submit-complaint")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]ComplaintDetails model)
        {
            try
            {
                var result = await _caseService.CreateComplaintCase(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController CreateCase an exception has occured while calling compliments complaints createCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }

        [Route("submit-complaint-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] PostData formAnswers)
        {
            try
            {
                var result = await _caseService.CreateComplaintCaseFormBuilder(formAnswers);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController CreateCaseUsingFormBuilder an exception has occured while calling compliments complaints createCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }
    }
}
