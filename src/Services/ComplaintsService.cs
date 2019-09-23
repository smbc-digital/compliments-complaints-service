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
    }
}
