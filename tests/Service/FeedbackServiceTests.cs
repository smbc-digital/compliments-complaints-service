using System;
using System.Net;
using compliments_complaints_service.Services;
using compliments_complaints_service.Utils;
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
        private readonly Mock<IEventCodesHelper> _mockEventCodeHelper = new Mock<IEventCodesHelper>();

        public FeedbackServiceTests()
        {
            _service = new FeedbackService(_mockGateway.Object, _mockEventCodeHelper.Object);
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
