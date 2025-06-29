namespace FSI.AccessAuthentication.Application.Messaging
{
    public class BaseMessage<T> where T : class
    {
        public string Action { get; set; }
        public T Payload { get; set; }
        public long MessagingId { get; set; }
    }
}
