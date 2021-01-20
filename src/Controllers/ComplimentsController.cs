using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
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

        public ComplimentsController(IComplimentsService caseService)
        {
            _caseService = caseService;
        }

        [Route("submit-compliment")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]ComplimentDetails model)
        {
            var result = await _caseService.CreateComplimentCase(model);

            return Ok(result);
        }

        [Route("submit-compliment-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody]ComplimentDetails model)
        {
            var result = await _caseService.CreateComplimentCase(model);

            return Ok(result);
        }
    }
}
