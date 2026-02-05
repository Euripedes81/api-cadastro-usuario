using System.Text.Json.Serialization;

namespace Application.Messages
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
