using FSI.AccessAuthentication.Application.Dtos;

namespace FSI.AccessAuthentication.Application.Messaging
{
    public class ProfileMessage
    {
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public ProfileDto Payload { get; set; } = new();
        public long MessagingId { get; set; } // Id da mensagem na tabela Messaging
    }
}
