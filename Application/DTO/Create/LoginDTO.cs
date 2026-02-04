using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Create
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
    }
}
