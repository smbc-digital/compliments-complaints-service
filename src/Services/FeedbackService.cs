using System;
using System.Linq;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Models;
using compliments_complaints_service.Mappers;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Verint;

namespace compliments_complaints_service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IOptions<FeedbackListConfiguration> _feedbackConfig;

        public FeedbackService(IVerintServiceGateway verintServiceGateway, IOptions<FeedbackListConfiguration> feedbackConfig)
        {
            _verintServiceGateway = verintServiceGateway;
            _feedbackConfig = feedbackConfig;
        }

        public async Task<string> CreateFeedbackCase(FeedbackDetails model)
        {
            var events = _feedbackConfig.Value.FeedbackConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            string name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;
            string email = string.IsNullOrEmpty(model.Email) ? "Not provided" : model.Email;

            var crmCase = new Case
            {
                EventCode = (int) eventCode,
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Feedback" : $"Feedback - {model.CouncilDepartmentOther}",
                Description = $"Name: {name} \nEmail: {email}\n\nFeedback: {model.Feedback}"
            };

            try
            {
                var response = await _verintServiceGateway.CreateCase(crmCase);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateFeedbackCase an exception has occured while creating the case in verint service", ex);
            }
        }

        public async Task<string> CreateFeedbackCaseFormBuilder(FeedbackAndComplimentDetailsFormBuilder model)
        {
            var crmCase = FeedBackAndComplimentsodelMapper.FeedbackToCrmCase(model, _feedbackConfig);

            try
            {
                var response = await _verintServiceGateway.CreateCase(crmCase);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateFeedbackCase an exception has occured while creating the case in verint service", ex);
            }
        }
    }
}
