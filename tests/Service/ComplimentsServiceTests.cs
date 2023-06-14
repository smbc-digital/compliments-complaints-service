using System.Collections.Generic;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.Options;
using Moq;
using StockportGovUK.NetStandard.Gateways.VerintService;

namespace compliments_complaints_service_tests.Service
{
    public class ComplimentsServiceTests
    {
        private readonly ComplimentsService _service;
        private readonly Mock<IVerintServiceGateway> _mockGateway = new Mock<IVerintServiceGateway>();
        private readonly Mock<IOptions<ComplimentsListConfiguration>> _mockComplimentsList = new Mock<IOptions<ComplimentsListConfiguration>>();


        public ComplimentsServiceTests()
        {
            var config = new ComplimentsListConfiguration
            {
                ComplimentsConfigurations = new List<ComplimentsConfiguration>
                {
                    new ComplimentsConfiguration
                    {
                        EventName = "test",
                        EventCode = 123456
                    },
                    new ComplimentsConfiguration
                    {
                        EventName = "none",
                        EventCode = 654321
                    }
                }
            };

            _mockComplimentsList.Setup(_ => _.Value).Returns(config);
            _service = new ComplimentsService(_mockGateway.Object, _mockComplimentsList.Object);
        }
    }
}
