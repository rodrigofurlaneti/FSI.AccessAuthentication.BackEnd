using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class AuthenticationDto : BaseDto
    {
        [JsonPropertyName("systemId")]
        public long SystemId { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("isAuthentication")]
        public bool IsAuthentication { get; set; }
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
    }
}
