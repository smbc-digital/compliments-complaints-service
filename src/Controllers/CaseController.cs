using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using compliments_complaints_service.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;

namespace fostering_service.Controllers.Case
{
    [Produces("application/json")]
    [Route("api/v1/Compliment")]
    [ApiController]
    [TokenAuthentication]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _caseService;
        private readonly ILogger<CaseController> _logger;

        public CaseController(ICaseService caseService, ILogger<CaseController> logger)
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
                //var result = await _caseService.GetCase(caseId);
                await _caseService.CreateComplimentCase(model);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"CaseController GetCase an exception has occured while calling compliments complaints getCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }
    }
}
