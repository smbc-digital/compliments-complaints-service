using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace compliments_complaints_service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(IVerintServiceGateway verintServiceGateway, ILogger<FeedbackService> logger)
        {
            _verintServiceGateway = verintServiceGateway;
            _logger = logger;
        }

        public async Task<string> CreateFeedbackCase(FeedbackDetails model)
        {
            var crmCase = new Case
            {
                EventCode = int.Parse(model.EventCode),
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentOther) ? "Feedback" : $"Feedback - {model.CouncilDepartmentOther}",
                Description = string.IsNullOrEmpty(model.Name) ? model.Feedback : $"{model.Feedback} - {model.Name} - {model.Email}"
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
