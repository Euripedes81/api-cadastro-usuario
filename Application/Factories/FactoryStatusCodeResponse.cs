using Application.Common.Responses;

namespace Application.Factories
{
    public class FactoryStatusCodeResponse
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
