using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PerfilUsuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nome { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}