using Application.DTO.Responses;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Generics;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {         
           services.AddScoped<IUsuarioAppService, UsuarioAppService>();
           services.AddScoped<IReaderAppService<PerfilUsuarioResponseDTO>, PerfilUsuarioAppService>(); 
           return services;
        }
    }
}
