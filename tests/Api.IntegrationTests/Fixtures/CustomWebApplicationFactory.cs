namespace Api.IntegrationTests.Fixtures;

using Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

public class CustomWebApplicationFactory : WebApplicationFactory<global::Program>
{
    public Mock<IUsuarioAppService> UsuarioServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IUsuarioAppService>();

            services.AddSingleton(UsuarioServiceMock.Object);
        });
    }
}
