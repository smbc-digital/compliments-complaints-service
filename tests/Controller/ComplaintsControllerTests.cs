using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using compliments_complaints_service.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace compliments_complaints_service_tests.Controller
{
    public class ComplaintsControllerTests
    {
        private readonly ComplaintsController _controller;
        private readonly Mock<IComplaintsService> _mockService = new Mock<IComplaintsService>();

        public ComplaintsControllerTests()
        {
            _controller = new ComplaintsController(_mockService.Object);
        }


        [Fact]
        public async void CreateCaseFormbuilder_ShouldCallService()
        {
            // Arrange
            _mockService
                .Setup(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetailsFormBuilder>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            await _controller.CreateCase(It.IsAny<ComplaintDetailsFormBuilder>());

            // Assert
            _mockService.Verify(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetailsFormBuilder>()), Times.Once);
        }

        [Fact]
        public async void CreateCaseFormBuilder_ShouldReturnOkResult()
        {
            // Arrange
            //
            _mockService
                .Setup(_ => _.CreateComplaintCase(It.IsAny<ComplaintDetailsFormBuilder>()))
                .ReturnsAsync(It.IsAny<string>());

            // Act
            var result = await _controller.CreateCase(It.IsAny<ComplaintDetailsFormBuilder>());


            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
