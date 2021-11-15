using System;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Models;
using compliments_complaints_service.Mappers;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;

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
