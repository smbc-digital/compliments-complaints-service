using System;
using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class ComplaintsControllerTests
    {
        private readonly ComplaintsController _controller;
        private readonly Mock<IComplaintsService> _mockService = new Mock<IComplaintsService>();
        private readonly Mock<ILogger<ComplaintsController>> _logger = new Mock<ILogger<ComplaintsController>>();

        public ComplaintsControllerTests()
        {
            _controller = new ComplaintsController(_logger.Object, _mockService.Object);
        }

        [Fact]
        public async void CreateCase_ShouldCallService()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            await _controller.CreateCase(It.IsAny<ComplaintDetails>());

            // Assert
            _mockService.Verify(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetails>()), Times.Once);
        }

        [Fact]
        public async void CreateCase_ShouldReturnOkResult()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            var result = await _controller.CreateCase(It.IsAny<ComplaintDetails>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateCase_ShouldReturn500()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetails>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateCase(It.IsAny<ComplaintDetails>());

            // Assert
            var assertType = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, assertType.StatusCode);
        }
    }
}
