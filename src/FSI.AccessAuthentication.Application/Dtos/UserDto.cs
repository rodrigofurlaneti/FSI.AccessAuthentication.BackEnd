using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class UserDto : BaseDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("systemId")]
        public long SystemId { get; set; }
        [JsonPropertyName("profileId")]
        public long ProfileId { get; set; }
    }
}
