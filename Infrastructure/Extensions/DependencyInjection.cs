using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Infraestruture.Security;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repository;
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
                options.UseSqlServer(configuration.GetConnectionString("Local")));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPerfilUsuarioRepository, PerfilUsuarioRepository>();         
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
