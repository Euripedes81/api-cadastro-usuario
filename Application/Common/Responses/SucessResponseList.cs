using System.Text.Json.Serialization;

namespace Application.Common.Responses
{
    public class SuccessResponseList<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; }

        public SuccessResponseList(List<T> data)
        {
            Data = data;
        }
    }
}
