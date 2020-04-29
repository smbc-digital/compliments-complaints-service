using System;
using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class FeedbackControllerTests
    {
        private readonly FeedbackController _controller;
        private readonly Mock<IFeedbackService> _mockService = new Mock<IFeedbackService>();

        public FeedbackControllerTests()
        {
            _controller = new FeedbackController(_mockService.Object);
        }

        [Fact]
        public async void CreateCase_ShouldCallService()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateFeedbackCase(It.IsAny<FeedbackDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            await _controller.CreateCase(It.IsAny<FeedbackDetails>());

            // Assert
            _mockService.Verify(_ => _.CreateFeedbackCase(It.IsAny<FeedbackDetails>()), Times.Once);
        }

        [Fact]
        public async void CreateCase_ShouldReturnOkResult()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateFeedbackCase(It.IsAny<FeedbackDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            var result =  await _controller.CreateCase(It.IsAny<FeedbackDetails>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
