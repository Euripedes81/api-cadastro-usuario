using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UsuarioDbContextFactory
    {
        public UsuarioDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsuarioDbContext>();
           
            optionsBuilder.UseSqlServer("Server=localhost;Database=CadastroUsuario;Trusted_Connection=True;TrustServerCertificate=True;");

            return new UsuarioDbContext(optionsBuilder.Options);
        }
    }
}
