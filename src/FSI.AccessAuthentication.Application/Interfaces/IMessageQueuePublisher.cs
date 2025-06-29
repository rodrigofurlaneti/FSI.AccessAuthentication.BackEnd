namespace FSI.AccessAuthentication.Application.Interfaces
{
    public interface IMessageQueuePublisher
    {
        void Publish<T>(T message, string queueName);
    }
}
