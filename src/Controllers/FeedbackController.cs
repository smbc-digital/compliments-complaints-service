using System.Threading.Tasks;
using compliments_complaints_service.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;

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
            => _caseService = caseService;

        [Route("submit-feedback-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] FeedbackAndComplimentDetailsFormBuilder model)
            => Ok(await _caseService.CreateFeedbackCaseFormBuilder(model));
    }
}
