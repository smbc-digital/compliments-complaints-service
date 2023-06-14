namespace compliments_complaints_service.Config
{
    public class ComplaintsConfiguration
    {
        public string EventName { get; set; }

        public int EventCode { get; set; }
    }

    public class ComplaintsListConfiguration
    {
        public List<ComplaintsConfiguration> ComplaintsConfigurations { get; set; }
    }
}