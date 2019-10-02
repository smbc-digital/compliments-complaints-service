using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using compliments_complaints_service.Config;
using System.Linq;
using System.Collections.Generic;
using compliments_complaints_service.Utils;

namespace compliments_complaints_service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IOptions<EventModel> _feedbackCodes;

        public FeedbackService(IVerintServiceGateway verintServiceGateway, ILogger<FeedbackService> logger, IOptions<EventModel> feedbackCodes)
        {
            _verintServiceGateway = verintServiceGateway;
            _logger = logger;
            _feedbackCodes = feedbackCodes;
        }

        public async Task<string> CreateFeedbackCase(FeedbackDetails model)
        {
            int eventCode;
            EventCodesHelper eventCodesHelper = new EventCodesHelper();

            eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? eventCodesHelper.getRealEventCode(model.CouncilDepartment, "feedback")
                : eventCodesHelper.getRealEventCode(model.CouncilDepartmentSub, "feedback");

            if (eventCode == 0) eventCode = 4000030; 

            string name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;
            string email = string.IsNullOrEmpty(model.Email) ? "Not provided" : model.Email;
            string description = string.Format("Name: {0} {1} Email: {2} {3} {4} Feedback: {5}", name, Environment.NewLine, email, Environment.NewLine, Environment.NewLine, model.Feedback);

            var crmCase = new Case
            {
                EventCode = eventCode,
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Feedback" : $"Feedback - {model.CouncilDepartmentOther}",
                Description = description
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
    }
}
