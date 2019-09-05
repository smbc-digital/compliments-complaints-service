using compliments_complaints_service.Models;
using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Net;
using System.Threading.Tasks;

namespace compliments_complaints_service.Services
{
    public class CaseService : ICaseService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
//        private readonly ILogger<HomeVisitService> _logger;
        private readonly string _integrationFormName = "Compliments";

        public CaseService(IVerintServiceGateway verintServiceGateway)
        {
            _verintServiceGateway = verintServiceGateway;
//            _logger = logger;
        }



        public async Task<bool> CreateComplimentCase(ComplimentDetails model)
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

            return true;
            //return completed ? ETaskStatus.Completed : ETaskStatus.NotCompleted;
        }

    }
}
