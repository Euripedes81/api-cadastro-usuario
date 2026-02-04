using System.Text.Json.Serialization;

namespace Application.Common.Responses
{
    public class ErrorResponse<T>
    {
        [JsonPropertyName("error")]
        public T Error { get; set; }

        public ErrorResponse(T error)
        {
            Error = error;
        }
    }
}
