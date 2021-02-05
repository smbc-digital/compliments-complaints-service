using System;
using System.Linq;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Mappers;
using compliments_complaints_service.Models;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Verint;

namespace compliments_complaints_service.Services
{
    public class ComplimentsService : IComplimentsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IOptions<ComplimentsListConfiguration> _complimentsConfig;

        public ComplimentsService(IVerintServiceGateway verintServiceGateway, IOptions<ComplimentsListConfiguration> complimentsConfig)
        {
            _verintServiceGateway = verintServiceGateway;
            _complimentsConfig = complimentsConfig;
        }

        public async Task<string> CreateComplimentCase(ComplimentDetails model)
        {
            var events = _complimentsConfig.Value.ComplimentsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                  : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            var name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;

            var crmCase = new Case
            {
                EventCode = (int) eventCode,
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Compliment" : $"Compliment - {model.CouncilDepartmentOther}",
                Description = $"Name: {name} \n\nCompliment: {model.Compliment}"
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

        public async Task<string> CreateComplimentCaseFormBuilder(FeedbackAndComplimentDetailsFormBuilder model)
        {
            var crmCase = FeedBackAndComplimentsodelMapper.ComplimentToCrmCase(model, _complimentsConfig);

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
