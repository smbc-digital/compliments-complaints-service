using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Feedback")]
    [ApiController]
    [TokenAuthentication]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _caseService;

        public FeedbackController(IFeedbackService caseService)
        {
            _caseService = caseService;
        }

        [Route("submit-feedback")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]FeedbackDetails model)
        {
            var result = await _caseService.CreateFeedbackCase(model);

            return Ok(result);
        }

        [Route("submit-feedback-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] PostData data)
        {
            var result = await _caseService.CreateFeedbackCaseFormBuilder(data);

            return Ok(result);
        }
    }
}
