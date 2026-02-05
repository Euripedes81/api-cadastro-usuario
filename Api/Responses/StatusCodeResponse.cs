using System.Text.Json.Serialization;

namespace Api.Responses
{
    public class StatusCodeResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }      
    }
}
