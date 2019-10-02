using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using compliments_complaints_service.Utils;

namespace compliments_complaints_service.Services
{
    public class ComplimentsService : IComplimentsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IEventCodesHelper _eventCodesHelper;

        public ComplimentsService(IVerintServiceGateway verintServiceGateway, IEventCodesHelper eventCodesHelper)
        {
            _verintServiceGateway = verintServiceGateway;
            _eventCodesHelper = eventCodesHelper;
        }

        public async Task<string> CreateComplimentCase(ComplimentDetails model)
        {
            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? _eventCodesHelper.getRealEventCode(model.CouncilDepartment, "compliments")
                : _eventCodesHelper.getRealEventCode(model.CouncilDepartmentSub, "compliments");

            if (eventCode == 0) eventCode = 4000000;

            var name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;
            var description = string.Format("Name: {0} {1} {2} Compliment: {3}", name, Environment.NewLine, Environment.NewLine, model.Compliment);


            var crmCase = new Case
            {
                EventCode = eventCode,
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Compliment" : $"Compliment - {model.CouncilDepartmentOther}",
                Description = description
            };

            try
            {
                var response = await _verintServiceGateway.CreateCase(crmCase);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateComplimentCase an exception has occured while creating the case in verint service", ex);
            }
        }
    }
}
