using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Net;
using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Services
{
    public class CaseService : ICaseService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
//        private readonly ILogger<HomeVisitService> _logger;

        public CaseService(IVerintServiceGateway verintServiceGateway)
        {
            _verintServiceGateway = verintServiceGateway;
//            _logger = logger;
        }



        public async Task<HttpResponse<CreateCaseResponse>> CreateComplimentCase(ComplimentDetails model)
        {
            var crmCase = new Case
            {
                EventCode = int.Parse(model.EventCode),
                EventTitle = "Compliment",
                Description = string.IsNullOrEmpty(model.Name) ? model.Compliment : $"{model.Compliment} - {model.Name}"
            };

            var response = await _verintServiceGateway.CreateCase(crmCase);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Create compliment failure");
            }

            return response;
        }

    }
}
