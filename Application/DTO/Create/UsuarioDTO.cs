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
        [MinLength(6, ErrorMessage = "Número mínimo de caracteres 6.")]
        [MaxLength(20, ErrorMessage = "Número máximo de caracteres 50.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{6,}$",
             ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial."
             )]
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
