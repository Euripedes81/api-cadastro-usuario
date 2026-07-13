using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UsuarioDbContextFactory
    {
        public UsuarioDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsuarioDbContext>();
           
            optionsBuilder.UseSqlServer("Server=tcp:cadastro.database.windows.net,1433;Initial Catalog=usuario;Persist Security Info=False;User ID=CloudSA75f512c9;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new UsuarioDbContext(optionsBuilder.Options);
        }
    }
}
