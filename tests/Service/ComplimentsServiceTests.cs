using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.AspNetCore.Gateways.Response;
using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using Xunit;

namespace compliments_complaints_service_tests.Service
{
    public class ComplimentsServiceTests
    {
        private readonly ComplimentsService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<ILogger<ComplimentsService>> _mockLogger = new Mock<ILogger<ComplimentsService>>();

        public ComplimentsServiceTests()
        {
            _service = new ComplimentsService(_mockGateway.Object, _mockLogger.Object);
        }

        [Fact]
        public async void CreateComplimentCase_ShouldCallGateway()
        {
            // Arrange 
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            var model = new ComplimentDetails
            {
                EventCode = "123456",
                Compliment = "test"
            };

            // Act
            await _service.CreateComplimentCase(model);

            // Assert
            _mockGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);
        }

        [Fact]
        public async void CreateComplimentCase_ShouldReturnCaseId()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            var model = new ComplimentDetails
            {
                EventCode = "123456",
                Compliment = "test"
            };

            // Act
            var response = await _service.CreateComplimentCase(model);

            // Assert
            Assert.Equal("123456", response);
        }

        [Fact]
        public async void CreateComplimentCase_ShouldThrowException()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .Throws<Exception>();

            var model = new ComplimentDetails
            {
                EventCode = "123456",
                Compliment = "test"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateComplimentCase(model));
        }


    }
}
