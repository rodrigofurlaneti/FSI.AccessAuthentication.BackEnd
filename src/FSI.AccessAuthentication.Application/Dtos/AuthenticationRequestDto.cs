using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class AuthenticationRequestDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
