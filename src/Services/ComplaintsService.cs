﻿using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using compliments_complaints_service.Utils;

namespace compliments_complaints_service.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly ILogger<ComplaintsService> _logger;

        public ComplaintsService(IVerintServiceGateway verintServiceGateway, ILogger<ComplaintsService> logger)
        {
            _verintServiceGateway = verintServiceGateway;
            _logger = logger;
        }

        public async Task<string> CreateComplaintCase(ComplaintDetails model)
        {
            int eventCode;
            EventCodesHelper eventCodesHelper = new EventCodesHelper();

            eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? eventCodesHelper.getRealEventCode(model.CouncilDepartment, "complaints")
                : eventCodesHelper.getRealEventCode(model.CouncilDepartmentSub, "complaints");

            if (eventCode == 0) eventCode = 2007854;

            var crmCase = new Case
            {
                EventCode = eventCode,
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
                if (splitAddress.Length == 5)
                {
                    crmCase.Customer.Address.AddressLine1 = splitAddress[0];
                    crmCase.Customer.Address.AddressLine2 = splitAddress[1];
                    crmCase.Customer.Address.AddressLine3 = splitAddress[2];
                    crmCase.Customer.Address.City = splitAddress[3];
                    crmCase.Customer.Address.Postcode = splitAddress[4];
                }
                else
                {
                    crmCase.Customer.Address.AddressLine1 = splitAddress[0];
                    crmCase.Customer.Address.AddressLine2 = splitAddress[1];
                    crmCase.Customer.Address.City = splitAddress[2];
                    crmCase.Customer.Address.Postcode = splitAddress[3];
                }
                crmCase.Customer.Address.UPRN = model.ContactDetails.Address.PlaceRef;
            }

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
