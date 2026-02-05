using System.Text.Json.Serialization;

namespace Api.Responses
{
    public class SuccessResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
        public string Message { get; set; }

        public SuccessResponse(T data, string message = "")
        {
            Data = data;
            Message = message;
        }
    }
}
