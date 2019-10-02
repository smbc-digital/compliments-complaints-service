using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.AspNetCore.Gateways.Response;
using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using StockportGovUK.NetStandard.Models.Models.Verint;
using Xunit;

namespace compliments_complaints_service_tests.Service
{
    public class FeedbackServiceTests
    {
        private readonly FeedbackService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<ILogger<FeedbackService>> _mockLogger = new Mock<ILogger<FeedbackService>>();
        private readonly Mock<IOptions<EventModel>> _feedbackCodes = new Mock<IOptions<EventModel>>();

        public FeedbackServiceTests()
        {
            _service = new FeedbackService(_mockGateway.Object, _mockLogger.Object, _feedbackCodes.Object);
        }

        [Fact(Skip = "Not reading the feedback.json file")]
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
                EventCode = "123456",
                Feedback = "test"
            };

            // Act
            await _service.CreateFeedbackCase(model);

            // Assert
            _mockGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);
        }

        [Fact(Skip = "Not reading the feedback.json file")]
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
                EventCode = "123456",
                Feedback = "test"
            };

            // Act
            var response = await _service.CreateFeedbackCase(model);

            // Assert
            Assert.Equal("123456", response);
        }

        [Fact(Skip = "Not reading the feedback.json file")]
        public async void CreateFeedbackCase_ShouldThrowException()
        {
            // Arrange
            _mockGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .Throws<Exception>();

            var model = new FeedbackDetails
            {
                EventCode = "123456",
                Feedback = "test"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateFeedbackCase(model));
        }
    }
}
