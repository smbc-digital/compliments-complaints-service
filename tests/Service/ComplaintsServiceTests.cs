using System;
using System.Collections.Generic;
using System.Net;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.NetStandard.Gateways.MailingServiceGateway;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.ContactDetails;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace compliments_complaints_service_tests.Service
{
    public class ComplaintsServiceTests
    {
        private readonly ComplaintsService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<IMailingServiceGateway> _mockMailingGateway = new Mock<IMailingServiceGateway>();
        private readonly Mock<IOptions<ComplaintsListConfiguration>> _mockComplaintsList = new Mock<IOptions<ComplaintsListConfiguration>>();
        private readonly Mock<ILogger<ComplaintsService>> _mockLogger = new Mock<ILogger<ComplaintsService>>();
        private readonly ComplaintDetails model = new ComplaintDetails
        {
            EventCode = "4000010",
            ComplainAboutService = "Bins",
            CouncilDepartment = "4000010",
            ComplainAboutDetails = "This is a test... Don't take it seriously",
            ContactDetails = new ContactDetails
            {
                Address = new StockportGovUK.NetStandard.Models.Addresses.Address()
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
            _service = new ComplaintsService(_mockGateway.Object, _mockComplaintsList.Object, _mockMailingGateway.Object, _mockLogger.Object);
        }

        [Fact]
        public async void CreateComplaintCase_ShouldCallGateway()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            // Act
            await _service.CreateComplaintCase(model);

            // Assert
            _mockGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);
        }

        [Fact]
        public async void CreateComplaintCase_ShouldReturnCaseId()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            // Act
            var result = await _service.CreateComplaintCase(model);

            // Assert
            Assert.Equal("123456", result);
        }

        [Fact]
        public async void CreateComplaintCase_ShouldThrowException()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateComplaintCase(model));
        }
    }
}
