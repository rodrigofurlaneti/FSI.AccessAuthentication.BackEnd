using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class MessagingDto : BaseDto
    {
        [JsonPropertyName("operationMessage")]
        public string OperationMessage { get; set; } = string.Empty;
        [JsonPropertyName("queueName")]
        public string QueueName { get; set; } = string.Empty;
        [JsonPropertyName("messageRequest")]
        public string MessageRequest { get; set; } = string.Empty;
        [JsonPropertyName("messageResponse")]
        public string MessageResponse { get; set; } = string.Empty;
        [JsonPropertyName("isProcessed")]
        public bool IsProcessed { get; set; } = false;
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty;

        public MessagingDto()
        {

        }

        public MessagingDto(long id)
        {
            Id = id;
        }

        public MessagingDto(string actionMessaging, string queueName, string messageRequest, bool isProcessed, string errorMessage)
        {
            OperationMessage = actionMessaging;
            QueueName = queueName;
            MessageRequest = messageRequest;
            IsProcessed = isProcessed;
            ErrorMessage = errorMessage;
        }

        public MessagingDto(string actionMessaging, string queueName, string messageRequest, string messageResponse, bool isProcessed, string errorMessage)
        {
            OperationMessage = actionMessaging;
            QueueName = queueName;
            MessageRequest = messageRequest;
            MessageResponse = messageResponse;
            IsProcessed = isProcessed;
            ErrorMessage = errorMessage;
        }
    }
}
