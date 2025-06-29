using System.Text.Json.Serialization;

namespace FSI.AccessAuthentication.Application.Dtos
{
    public abstract class BaseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
