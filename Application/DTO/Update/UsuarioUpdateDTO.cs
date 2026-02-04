using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Create
{
    public class UsuarioUpdateDTO
    {
        [Required]
        public string? Nome { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Número mínimo de caracteres 6.")]
        [MaxLength(50, ErrorMessage = "Número máximo de caracteres 20.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{6,}$",
             ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial."
             )]
        public string? Senha { get; set; }
        [Required]
        public PerfilUsuarioDTO? Perfil { get; set; }
    }  
}
