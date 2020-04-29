using System;
using System.Collections.Generic;
using System.Net;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;

namespace compliments_complaints_service_tests.Service
{
    public class FeedbackServiceTests
    {
        private readonly FeedbackService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<IOptions<FeedbackListConfiguration>> _mockFeedbackList = new Mock<IOptions<FeedbackListConfiguration>>();

        public FeedbackServiceTests()
        {
            var config = new FeedbackListConfiguration
            {
                FeedbackConfigurations = new List<FeedbackConfiguration>
                {
                    new FeedbackConfiguration
                    {
                        EventName = "test",
                        EventCode = 123456
                    },
                    new FeedbackConfiguration
                    {
                        EventName = "none",
                        EventCode = 654321
                    }
                }
            };

            _mockFeedbackList.Setup(_ => _.Value).Returns(config);
            _service = new FeedbackService(_mockGateway.Object, _mockFeedbackList.Object);
        }

        [Fact]
        public async void CreateFeedbackCase_ShouldCallGateway()
        {
            // Arrange 
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            var model = new FeedbackDetails
            {
                CouncilDepartment = "test",
                Feedback = "test"
            };

            // Act
            await _service.CreateFeedbackCase(model);

            // Assert
            _mockGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);
        }

        [Fact]
        public async void CreateFeedbackCase_ShouldReturnCaseId()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseContent = "123456"
                });

            var model = new FeedbackDetails
            {
                CouncilDepartment = "none",
                Feedback = "test"
            };

            // Act
            var response = await _service.CreateFeedbackCase(model);

            // Assert
            Assert.Equal("123456", response);
        }

        [Fact]
        public async void CreateFeedbackCase_ShouldThrowException()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .Throws<Exception>();

            var model = new FeedbackDetails
            {
                CouncilDepartment = "test",
                Feedback = "test"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateFeedbackCase(model));
        }
    }
}
