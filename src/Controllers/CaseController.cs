using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using compliments_complaints_service.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;

namespace fostering_service.Controllers.Case
{
    [Produces("application/json")]
    [Route("api/v1/[Controller]")]
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

        [Route("case")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromQuery]List<Answer> data)
        {
            try
            {
                //var result = await _caseService.GetCase(caseId);

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
