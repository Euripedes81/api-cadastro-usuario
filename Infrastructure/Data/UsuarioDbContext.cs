using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UsuarioDbContext : DbContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options) { }      
        public DbSet<Usuario> Usuarios { get; set; }      
        public DbSet<PerfilUsuario> PerfilUsuarios { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario")
                .HasOne(u => u.PerfilUsuario)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(u => u.PerfilUsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();      
           
            modelBuilder.Entity<PerfilUsuario>().ToTable("PerfilUsuario");          

            modelBuilder.Entity<PerfilUsuario>().HasData(
                new PerfilUsuario { Id = 1, Nome = "Administrador" },
                new PerfilUsuario { Id = 2, Nome = "Usuário" }
                );

            var hash = new PasswordHasher<Usuario>();
            var usuario = new Usuario { Id = 1, Nome = "Administrador", Email = "admin@cadastro.com.br", Senha = "Admin07!Cadastro#", PerfilUsuarioId = 1, Inativo = false };
            usuario.Senha = hash.HashPassword(usuario, usuario.Senha);

            modelBuilder.Entity<Usuario>().HasData(usuario);
        }
    }
}