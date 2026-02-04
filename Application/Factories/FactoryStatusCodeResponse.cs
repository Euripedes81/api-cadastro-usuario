using Application.Common.Responses;

namespace Application.Factories
{
    public class FactoryStatusCodeResponse
    {
        public static StatusCodeResponse Create(string message = "", int status = 0)
        {
            return new StatusCodeResponse
            {
                Message = message,
                Code = status
            };         
        }
    }
}
