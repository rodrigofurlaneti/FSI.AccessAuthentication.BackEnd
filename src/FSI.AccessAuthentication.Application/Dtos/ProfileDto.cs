using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class ProfileDto : BaseDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
