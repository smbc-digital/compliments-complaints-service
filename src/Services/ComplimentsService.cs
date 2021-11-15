using System;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Mappers;
using compliments_complaints_service.Models;
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
