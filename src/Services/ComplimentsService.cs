using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Services
{
    public class ComplimentsService : IComplimentsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly ILogger<ComplimentsService> _logger;

        public ComplimentsService(IVerintServiceGateway verintServiceGateway, ILogger<ComplimentsService> logger)
        {
            _verintServiceGateway = verintServiceGateway;
            _logger = logger;
        }

        public async Task<HttpResponse<CreateCaseResponse>> CreateComplimentCase(ComplimentDetails model)
        {
            var crmCase = new Case
            {
                EventCode = int.Parse(model.EventCode),
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Compliment" : $"Compliment - {model.CouncilDepartmentOther}",
                Description = string.IsNullOrEmpty(model.Name) ? model.Compliment : $"{model.Compliment} - {model.Name}"
            };

            var response = await _verintServiceGateway.CreateCase(crmCase);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogWarning($"ComplimentsComplaintsService CreateComplimentCase an exception has occured while creating the case in verint service, statuscode: {response.StatusCode}");
                throw new Exception("Create compliment failure");
            }

            return response;
        }
    }
}
