using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using compliments_complaints_service.Utils;
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
        private readonly Mock<IEventCodesHelper> _mockEventCodeHelper = new Mock<IEventCodesHelper>();

        public FeedbackServiceTests()
        {
            _service = new FeedbackService(_mockGateway.Object, _mockLogger.Object, _mockEventCodeHelper.Object);
        }

        [Fact]
        //[Fact(Skip = "Not reading the feedback.json file")]
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

            _mockEventCodeHelper
                .Setup(_ => _.getRealEventCode(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(123456);

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

        //[Fact(Skip = "Not reading the feedback.json file")]
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

            _mockEventCodeHelper
                .Setup(_ => _.getRealEventCode(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(123456);

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

        //[Fact(Skip = "Not reading the feedback.json file")]
        [Fact]
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
