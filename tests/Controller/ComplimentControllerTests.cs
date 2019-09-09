using System;
using System.Collections.Generic;
using System.Text;
using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.AspNetCore.Gateways.Response;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class ComplimentControllerTests
    {
        private readonly ComplimentController _controller;
        private readonly Mock<ICaseService> _mockService = new Mock<ICaseService>();
        private readonly Mock<ILogger<ComplimentController>> _logger = new Mock<ILogger<ComplimentController>>();

        public ComplimentControllerTests()
        {
            _controller = new ComplimentController(_mockService.Object, _logger.Object);
        }

        [Fact]
        public async void CreateCase_ShouldCallService()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplimentCase(It.IsAny<ComplimentDetails>()))
                .ReturnsAsync(It.IsAny<HttpResponse<CreateCaseResponse>>());

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
                .ReturnsAsync(new HttpResponse<CreateCaseResponse>());

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
