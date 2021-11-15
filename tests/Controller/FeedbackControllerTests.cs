using compliments_complaints_service.Controllers;
using compliments_complaints_service.Services;
using Moq;

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

    }
}
