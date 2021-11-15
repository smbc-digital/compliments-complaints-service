using System.Threading.Tasks;
using compliments_complaints_service.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;

namespace compliments_complaints_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Compliments")]
    [ApiController]
    [TokenAuthentication]
    public class ComplimentsController : ControllerBase
    {
        private readonly IComplimentsService _caseService;

        public ComplimentsController(IComplimentsService caseService)
            => _caseService = caseService;

        [Route("submit-compliment-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] FeedbackAndComplimentDetailsFormBuilder model)
            => Ok(await _caseService.CreateComplimentCaseFormBuilder(model));
    }
}
