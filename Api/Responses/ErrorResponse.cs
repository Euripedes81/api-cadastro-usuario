using System.Text.Json.Serialization;

namespace Api.Responses
{
    public class ErrorResponse
    {
        public string Message { get; }
        public string? ErrorCode { get; }

        public ErrorResponse(string message, string? errorCode = "")
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
