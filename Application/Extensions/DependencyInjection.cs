using Application.Interfaces.IServices;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {         
           services.AddScoped<IUsuarioAppService, UsuarioAppService>();         
           return services;
        }
    }
}
