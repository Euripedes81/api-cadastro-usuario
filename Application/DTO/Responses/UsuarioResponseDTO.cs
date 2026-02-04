namespace Application.DTO.Responses
{
    public record UsuarioResponseDTO : ErrorMessageDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public bool Inativo { get; set; }
        public PerfilUsuarioResponseDTO? Perfil { get; set; }

    }

    public class PerfilUsuarioResponseDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
}
