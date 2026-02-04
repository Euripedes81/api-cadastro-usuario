using System.Text.Json.Serialization;

namespace Application.Common.Responses
{
    public class StatusCodeResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("status")]
        public int Code { get; set; }      
    }
}
