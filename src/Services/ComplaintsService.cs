using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using compliments_complaints_service.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using StockportGovUK.AspNetCore.Gateways.MailingServiceGateway;
using StockportGovUK.AspNetCore.Gateways.Response;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Models.Mail;

namespace compliments_complaints_service.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IOptions<ComplaintsListConfiguration> _complaintsConfig;
        private readonly IMailingServiceGateway _mailingServiceGateway;

        public ComplaintsService(IVerintServiceGateway verintServiceGateway, IOptions<ComplaintsListConfiguration> complaintsConfig, IMailingServiceGateway mailingServiceGateway)
        {
            _verintServiceGateway = verintServiceGateway;
            _complaintsConfig = complaintsConfig;
            _mailingServiceGateway = mailingServiceGateway;
        }

        public async Task<string> CreateComplaintCase(ComplaintDetails model)
        {
            var events = _complaintsConfig.Value.ComplaintsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                  : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            var crmCase = new Case
            {
                EventCode = (int) eventCode,
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
                var response = await _verintServiceGateway.CreateCase(crmCase);
                SendUserSuccessEmail(model, response.ResponseContent);
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
    }
}
