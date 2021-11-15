using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Moq;

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

    }
}
