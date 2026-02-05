namespace Api.Responses
{
    public class StatusCodeResponseCreator
    {
        public static StatusCodeResponse Create(string message = "", int code = 0)
        {
            return new StatusCodeResponse
            {
                Message = message,
                Code = code
            };         
        }
    }
}
