using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Gateways.Response;

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
            var crmCase = new Case
            {
                EventCode = int.Parse(model.EventCode),
                EventTitle = string.IsNullOrEmpty(model.OtherService) ? $"Complaint - {model.ComplainAboutService}" : $"Complaint - {model.OtherService} - {model.ComplainAboutService}",
                Description = model.ComplainAboutDetails,
                Customer = new Customer
                {
                    Forename = model.FirstName,
                    Surname = model.LastName,
                    Email = model.EmailAddress,
                    Telephone = model.PhoneNumber,
                    Address = model.Address
                }
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
