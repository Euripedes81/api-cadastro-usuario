using Api.IntegrationTests.Fixtures;
using Api.Responses;
using Application.Common;
using Application.DTO.Create;
using Application.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Api.IntegrationTests.Controllers;

public class LoginEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public LoginEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostLogin_DeveRetornar200_QuandoCredenciaisValidas()
    {
        // Arrange
        var loginResponse = Activator.CreateInstance<LoginResponseDTO>()!;

        _factory.UsuarioServiceMock
            .Setup(x => x.FazerLoginAsync(
                It.IsAny<LoginDTO>()))
            .ReturnsAsync(
                ApplicationResult<LoginResponseDTO>
                    .Success(loginResponse));

        var login = new
        {
            Email = "teste@email.com",
            Senha = "Senha123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/v1/usuarios/logins", login);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultado =  await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

        Assert.NotNull(resultado);
       
        _factory.UsuarioServiceMock.Verify(
            x => x.FazerLoginAsync(
                It.Is<LoginDTO>(login =>
                    login.Email == "teste@email.com" &&
                    login.Senha == "Senha123!")),
            Times.Once);
    }

    [Fact]
    public async Task PostLogin_DeveRetornar401_QuandoCredenciaisInvalidas()
    {
        // Arrange
        _factory.UsuarioServiceMock
            .Setup(x => x.FazerLoginAsync(
                It.IsAny<LoginDTO>()))
            .ReturnsAsync(
                ApplicationResult<LoginResponseDTO>
                    .Failure(StatusCodes.Status401Unauthorized));

        var login = new
        {
            Email = "teste@email.com",
            Senha = "SenhaInvalida1!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/v1/usuarios/logins", login);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var resultado = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(resultado);

        Assert.Equal(StatusCodes.Status401Unauthorized, resultado.Status);

        Assert.Equal(MessageResponse.CredenciaisInvalidas, resultado.Title);

        Assert.Equal(MessageResponse.EmailSenhaInvalidos, resultado.Detail);
    }

    [Fact]
    public async Task PostLogin_DeveRetornar500_QuandoOcorrerErroInterno()
    {
        // Arrange
        _factory.UsuarioServiceMock
            .Setup(x => x.FazerLoginAsync(
                It.IsAny<LoginDTO>()))
            .ReturnsAsync(
                ApplicationResult<LoginResponseDTO>
                    .Failure(StatusCodes.Status500InternalServerError));

        var login = new
        {
            Email = "teste@email.com",
            Senha = "Senha123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/v1/usuarios/logins", login);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var resultado = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(resultado);

        Assert.Equal(StatusCodes.Status500InternalServerError, resultado.Status);

        Assert.Equal(MessageResponse.ErroInternoServidor, resultado.Title);
    }
}