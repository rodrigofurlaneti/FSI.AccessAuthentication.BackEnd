using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public class SystemDto : BaseDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; };
    }
}
