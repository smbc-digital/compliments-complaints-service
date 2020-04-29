using System;
using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class ComplimentsControllerTests
    {
        private readonly ComplimentsController _controller;
        private readonly Mock<IComplimentsService> _mockService = new Mock<IComplimentsService>();

        public ComplimentsControllerTests()
        {
            _controller = new ComplimentsController(_mockService.Object);
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
    }
}
