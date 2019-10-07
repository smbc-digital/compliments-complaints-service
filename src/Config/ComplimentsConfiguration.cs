using System.Collections.Generic;

namespace compliments_complaints_service.Config
{
    public class ComplimentsConfiguration
    {
        public string EventName { get; set; }

        public int EventCode { get; set; }
    }

    public class ComplimentsListConfiguration
    {
        public List<ComplimentsConfiguration> ComplimentsConfigurations { get; set; }
    }
}