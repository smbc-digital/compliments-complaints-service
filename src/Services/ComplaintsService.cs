using compliments_complaints_service.Config;
using compliments_complaints_service.Mappers;
using compliments_complaints_service.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StockportGovUK.NetStandard.Gateways.Enums;
using StockportGovUK.NetStandard.Gateways.MailingService;
using StockportGovUK.NetStandard.Gateways.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Gateways.Models.Mail;
using StockportGovUK.NetStandard.Gateways.VerintService;

namespace compliments_complaints_service.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IOptions<ComplaintsListConfiguration> _complaintsConfig;
        private readonly IMailingServiceGateway _mailingServiceGateway;

        public ComplaintsService(
            IVerintServiceGateway verintServiceGateway,
            IOptions<ComplaintsListConfiguration> complaintsConfig,
            IMailingServiceGateway mailingServiceGateway)
        {
            _verintServiceGateway = verintServiceGateway;
            _complaintsConfig = complaintsConfig;
            _mailingServiceGateway = mailingServiceGateway;
        }

        public async Task<string> CreateComplaintCase(ComplaintDetailsFormBuilder model)
        {
            var crmCase = ComplaintModelMapper.ToCrmCase(model, _complaintsConfig);
            try
            {
                var response = await _verintServiceGateway.CreateCase(crmCase);
                SendUserSuccessEmail(model, response.ResponseContent);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateComplimentCase an exception has occured while creating the case in verint service", ex);
            }
        }

        private void SendUserSuccessEmail(ComplaintDetailsFormBuilder model, string caseResponse)
        {
            var submissionDetails = new ComplaintsMailModel()
            {
                Subject = "We've received your formal complaint",
                Reference = caseResponse,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RecipientAddress = model.EmailAddress
            };

            _mailingServiceGateway.Send(new Mail
            {
                Payload = JsonConvert.SerializeObject(submissionDetails),
                Template = EMailTemplate.ComplaintsSuccess
            });
        }
    }
}
