using System;
using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Feedback")]
    [ApiController]
    [TokenAuthentication]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _caseService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackService caseService, ILogger<FeedbackController> logger)
        {
            _caseService = caseService;
            _logger = logger;
        }

        [Route("submit-feedback")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]FeedbackDetails model)
        {
            try
            {
                var result = await _caseService.CreateFeedbackCase(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController CreateCase an exception has occured while calling compliments complaints createCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }

        [Route("submit-feedback-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] PostData data)
        {
            try
            {
                var result = await _caseService.CreateFeedbackCaseFormBuilder(data);

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
