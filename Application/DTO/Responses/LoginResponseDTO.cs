namespace Application.DTO.Responses
{
    public record LoginResponseDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }

    }
}
