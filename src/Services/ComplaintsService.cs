using System;
using System.Linq;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using compliments_complaints_service.Mappers;
using compliments_complaints_service.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Gateways.MailingServiceGateway;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Mail;
using StockportGovUK.NetStandard.Models.Verint;
using Newtonsoft.Json;

namespace compliments_complaints_service.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IOptions<ComplaintsListConfiguration> _complaintsConfig;
        private readonly IMailingServiceGateway _mailingServiceGateway;
        private readonly ILogger<ComplaintsService> _logger;

        public ComplaintsService(
            IVerintServiceGateway verintServiceGateway, 
            IOptions<ComplaintsListConfiguration> complaintsConfig, 
            IMailingServiceGateway mailingServiceGateway,
            ILogger<ComplaintsService> logger)
        {
            _verintServiceGateway = verintServiceGateway;
            _complaintsConfig = complaintsConfig;
            _mailingServiceGateway = mailingServiceGateway;
            _logger = logger;
        }

        public async Task<string> CreateComplaintCase(ComplaintDetails model)
        {
            var events = _complaintsConfig.Value.ComplaintsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                  : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            var crmCase = new Case
            {                          
                EventCode = (int)eventCode,
                EventTitle = string.IsNullOrEmpty(model.OtherService) ? $"Complaint - {model.ComplainAboutService}" : $"Complaint - {model.OtherService} - {model.ComplainAboutService}",
                Description = model.ComplainAboutDetails,
                Customer = new Customer
                {
                    Forename = model.ContactDetails.FirstName,
                    Surname = model.ContactDetails.LastName,
                    Email = model.ContactDetails.EmailAddress,
                    Telephone = model.ContactDetails.PhoneNumber,
                    Address = new Address()
                }
            };

            if (string.IsNullOrEmpty(model.ContactDetails.Address.PlaceRef))
            {
                crmCase.Customer.Address.AddressLine1 = model.ContactDetails.Address.AddressLine1;
                crmCase.Customer.Address.AddressLine2 = model.ContactDetails.Address.AddressLine2;
                crmCase.Customer.Address.City = model.ContactDetails.Address.Town;
                crmCase.Customer.Address.Postcode = model.ContactDetails.Address.Postcode;
            }
            else
            {
                var splitAddress = model.ContactDetails.Address.SelectedAddress.Split(",");
                crmCase.Customer.Address.AddressLine1 = splitAddress[0];
                crmCase.Customer.Address.AddressLine2 = splitAddress[1];

                if (splitAddress.Length == 5)
                {    
                    crmCase.Customer.Address.AddressLine3 = splitAddress[2];
                    crmCase.Customer.Address.City = splitAddress[3];
                    crmCase.Customer.Address.Postcode = splitAddress[4];
                }
                else
                {
                    crmCase.Customer.Address.City = splitAddress[2];
                    crmCase.Customer.Address.Postcode = splitAddress[3];
                }

                crmCase.Customer.Address.UPRN = model.ContactDetails.Address.PlaceRef;
            }

            try
            {
                _logger.LogWarning($"ComplaintsService.CreateComplaintCase: Attempting to create verint case. {JsonConvert.SerializeObject(crmCase)}");
                var response = await _verintServiceGateway.CreateCase(crmCase);
                SendUserSuccessEmail(model, response.ResponseContent);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateComplimentCase an exception has occured while creating the case in verint service", ex);
            }
        }

        public async Task<string> CreateComplaintCaseFormBuilder(ComplaintDetailsFormBuilder model)
        {

            if (model.CustomersAddress != null)
            {
                _logger.LogWarning(model.CustomersAddress.AddressLine1);
                _logger.LogWarning(model.CustomersAddress.AddressLine2);
                _logger.LogWarning(model.CustomersAddress.Town);
                _logger.LogWarning(model.CustomersAddress.Postcode);
            }
            else
            {
                _logger.LogWarning("Customersaddress is null");
            }

            _logger.LogWarning(model.ComplaintStart);
            _logger.LogWarning(model.EmailAddress);
            _logger.LogWarning(model.PhoneNumber);
            _logger.LogWarning(model.ComplaintAbout);
            _logger.LogWarning(model.ComplaintAboutDetails);
            _logger.LogWarning(model.CouncilDepartment);
            _logger.LogWarning(model.CouncilDepartmentSub);
            _logger.LogWarning(model.FirstName);
            _logger.LogWarning(model.LastName);

            var crmCase = ComplaintModelMapper.ToCrmCase(model, _complaintsConfig);
            try
            {
                _logger.LogWarning($"ComplaintsService.CreateComplaintCase: Attempting to create verint case. {JsonConvert.SerializeObject(crmCase)}");
                var response = await _verintServiceGateway.CreateCase(crmCase);
                SendUserSuccessEmailFormBuilder(model, response.ResponseContent);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"ComplimentsComplaintsService CreateComplimentCase an exception has occured while creating the case in verint service", ex);
            }
        }

        private void SendUserSuccessEmail(ComplaintDetails model, string caseResponse)
        {
            var submissionDetails = new ComplaintsMailModel()
            {
                Subject = "We've received your formal complaint",
                Reference = caseResponse,
                FirstName = model.ContactDetails.FirstName,
                LastName = model.ContactDetails.LastName,
                RecipientAddress = model.ContactDetails.EmailAddress
            };

            _mailingServiceGateway.Send(new Mail
            {
                Payload = JsonConvert.SerializeObject(submissionDetails),
                Template = EMailTemplate.ComplaintsSuccess
            });

        }

        private void SendUserSuccessEmailFormBuilder(ComplaintDetailsFormBuilder model, string caseResponse)
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
