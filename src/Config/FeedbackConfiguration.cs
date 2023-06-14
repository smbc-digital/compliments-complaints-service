namespace compliments_complaints_service.Config
{
    public class FeedbackConfiguration
    {
        public string EventName { get; set; }

        public int EventCode { get; set; }
    }

    public class FeedbackListConfiguration
    {
        public List<FeedbackConfiguration> FeedbackConfigurations { get; set; }
    }
}
