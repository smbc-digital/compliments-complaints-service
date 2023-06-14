using System.Collections.Generic;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.NetStandard.Gateways.MailingService;
using StockportGovUK.NetStandard.Gateways.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Gateways.Models.ContactDetails;
using StockportGovUK.NetStandard.Gateways.VerintService;

namespace compliments_complaints_service_tests.Service
{
    public class ComplaintsServiceTests
    {
        private readonly ComplaintsService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<IMailingServiceGateway> _mockMailingGateway = new Mock<IMailingServiceGateway>();
        private readonly Mock<IOptions<ComplaintsListConfiguration>> _mockComplaintsList = new Mock<IOptions<ComplaintsListConfiguration>>();
        private readonly ComplaintDetails model = new ComplaintDetails
        {
            EventCode = "4000010",
            ComplainAboutService = "Bins",
            CouncilDepartment = "4000010",
            ComplainAboutDetails = "This is a test... Don't take it seriously",
            ContactDetails = new ContactDetails
            {
                Address = new StockportGovUK.NetStandard.Gateways.Models.Addresses.Address()
            }
        };

        public ComplaintsServiceTests()
        {
            var config = new ComplaintsListConfiguration()
            {
                ComplaintsConfigurations = new List<ComplaintsConfiguration>
                {
                    new ComplaintsConfiguration()
                    {
                        EventName = "test",
                        EventCode = 123456
                    },
                    new ComplaintsConfiguration
                    {
                        EventName = "none",
                        EventCode = 654321
                    }
                }
            };

            _mockComplaintsList.Setup(_ => _.Value).Returns(config);
            _service = new ComplaintsService(_mockGateway.Object, _mockComplaintsList.Object, _mockMailingGateway.Object);
        }

    }
}
