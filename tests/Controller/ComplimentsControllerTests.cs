using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class ComplimentsControllerTests
    {
        private readonly ComplimentsController _controller;
        private readonly Mock<IComplimentsService> _mockService = new Mock<IComplimentsService>();
        private readonly Mock<ILogger<ComplimentsController>> _logger = new Mock<ILogger<ComplimentsController>>();

        public ComplimentsControllerTests()
        {
            _controller = new ComplimentsController(_mockService.Object, _logger.Object);
        }

        [Fact]
        public async void CreateCase_ShouldCallService()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplimentCase(It.IsAny<ComplimentDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            await _controller.CreateCase(It.IsAny<ComplimentDetails>());

            // Assert
            _mockService.Verify(_ => _.CreateComplimentCase(It.IsAny<ComplimentDetails>()), Times.Once);
        }

        [Fact]
        public async void CreateCase_ShouldReturnOkResult()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplimentCase(It.IsAny<ComplimentDetails>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            var result =  await _controller.CreateCase(It.IsAny<ComplimentDetails>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateCase_ShouldReturn500()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplimentCase(It.IsAny<ComplimentDetails>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateCase(It.IsAny<ComplimentDetails>());

            // Assert
            var assertType = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, assertType.StatusCode);
        }
    }
}
