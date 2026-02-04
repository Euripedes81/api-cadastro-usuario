using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Senha { get; set; }      
        public bool Inativo { get; set; }   
        [Required]
        public int PerfilUsuarioId { get; set; }
        public PerfilUsuario? PerfilUsuario { get; set; }
      
    }
}