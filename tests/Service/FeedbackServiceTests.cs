using System.Collections.Generic;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.NetStandard.Gateways.VerintService;

namespace compliments_complaints_service_tests.Service
{
    public class FeedbackServiceTests
    {
        private readonly FeedbackService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<IOptions<FeedbackListConfiguration>> _mockFeedbackList = new Mock<IOptions<FeedbackListConfiguration>>();

        public FeedbackServiceTests()
        {
            var config = new FeedbackListConfiguration
            {
                FeedbackConfigurations = new List<FeedbackConfiguration>
                {
                    new FeedbackConfiguration
                    {
                        EventName = "test",
                        EventCode = 123456
                    },
                    new FeedbackConfiguration
                    {
                        EventName = "none",
                        EventCode = 654321
                    }
                }
            };

            _mockFeedbackList.Setup(_ => _.Value).Returns(config);
            _service = new FeedbackService(_mockGateway.Object, _mockFeedbackList.Object);
        }
    }
}
