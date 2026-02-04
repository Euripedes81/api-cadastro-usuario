namespace Application.DTO.Responses
{
    public record AtualizadoDTO : ErrorMessageDTO
    {
        public int Id { get; init; }       
    }
}
