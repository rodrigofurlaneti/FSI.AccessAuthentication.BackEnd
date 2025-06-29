using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class AuthenticationDto : BaseDto
    {
        [JsonPropertyName("systemId")]
        public int SystemId { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; };
        [JsonPropertyName("password")]
        public string Password { get; set; };
        [JsonPropertyName("isAuthentication")]
        public bool isAuthentication { get; set; }
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
    }
}
