using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Create
{
    public class UsuarioDTO
    {
        [Required]
        public string? Nome { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
        [Required]
        public PerfilUsuarioDTO? Perfil { get; set; }
        [Required]
        public bool Inativo { get; set; }
    }

    public class PerfilUsuarioDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
