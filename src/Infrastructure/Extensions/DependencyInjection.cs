using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Infraestruture.Security;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsuarioDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Remoto")));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IGenericRepository<PerfilUsuario>, EfRepository<PerfilUsuario>>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();

            return services;
        }
    }
}
