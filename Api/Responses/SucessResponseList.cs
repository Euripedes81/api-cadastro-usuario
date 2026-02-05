using System.Text.Json.Serialization;

namespace Api.Responses
{
    public class SuccessResponseList<T>
    {      
        public List<T> Data { get; set; }

        public SuccessResponseList(List<T> data)
        {
            Data = data;
        }
    }
}
