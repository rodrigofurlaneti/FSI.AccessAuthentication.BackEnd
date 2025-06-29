using FSI.AccessAuthentication.Application.Dtos;

namespace FSI.AccessAuthentication.Application.Messaging
{
    public class AuthenticationMessage
    {
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public AuthenticationDto Payload { get; set; } = new();
        public long MessagingId { get; set; } // Id da mensagem na tabela Messaging
    }
}
