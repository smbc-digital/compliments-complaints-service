using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Linq;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Mappers;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;

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
            var description = string.Format("Name: {0} {1} {2} Compliment: {3}", name, Environment.NewLine, Environment.NewLine, model.Compliment);

            var crmCase = new Case
            {
                EventCode = (int) eventCode,
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

        public async Task<string> CreateComplimentCaseFormBuilder(PostData data)
        {
            var model = FeedbackAndComplimentModelMapper.MapAnswers(data.Answers);

            var events = _complimentsConfig.Value.ComplimentsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            var name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;
            var description = string.Format("Name: {0} {1} {2} Compliment: {3}", name, Environment.NewLine, Environment.NewLine, model.Description);

            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = string.IsNullOrEmpty(model.OtherService) ? "Compliment" : $"Compliment - {model.OtherService}",
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
