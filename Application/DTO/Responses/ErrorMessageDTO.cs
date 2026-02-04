using System.Text.Json.Serialization;

namespace Application.DTO.Responses
{
    public abstract record ErrorMessageDTO
    {
        [JsonIgnore]
        public string Mensagem { get; init; } = string.Empty;

        [JsonIgnore]
        public int Code { get; init; }
    }
}
