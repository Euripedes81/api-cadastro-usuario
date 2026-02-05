namespace Application.DTO.Responses
{
    public record GenericResponseDTO : ErrorMessageDTO
    {
        public int Id { get; init; }       
    }
}
