﻿using System.Threading.Tasks;
using compliments_complaints_service.Models;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
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

        public ComplaintsController(IComplaintsService caseService)
        {
            _caseService = caseService;
        }

        [Route("submit-complaint")]
        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody]ComplaintDetails model)
        {
            var result = await _caseService.CreateComplaintCase(model);

            return Ok(result);
        }

        [Route("submit-complaint-form-builder")]
        [HttpPost]
        public async Task<IActionResult> CreateCaseUsingFormBuilder([FromBody] ComplaintDetailsFormBuilder model)
        {
            
            
            var result = await _caseService.CreateComplaintCaseFormBuilder(model);

            return Ok(result);
        }
    }
}
