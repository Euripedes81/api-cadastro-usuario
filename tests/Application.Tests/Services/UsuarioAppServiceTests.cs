using Application.Common;
using Application.DTO.Create;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Services
{
    public class UsuarioAppServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHasherService> _passwordHasherMock;
        private readonly UsuarioAppService _service;

        public UsuarioAppServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasherMock = new Mock<IPasswordHasherService>();

            _service = new UsuarioAppService( _usuarioRepositoryMock.Object, _tokenServiceMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public void Constructor_DeveCriarInstancia()
        {
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoUsuarioNaoExiste_DeveRetornarNotFound()
        {
            // Arrange

            _usuarioRepositoryMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync((Usuario?)null);

            // Act

            var result = await _service.ObterPorIdAsync(1);

            // Assert

            result.IsSuccess.Should().BeFalse();

            result.ErrorCode.Should().Be(ApplicationErrors.NotFound);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoUsuarioExiste_DeveRetornarDto()
        {
            // Arrange

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "João",
                Email = "joao@email.com",
                Senha = "HASH",
                PerfilUsuario = new PerfilUsuario { Id = 2, Nome = "Usuário" }
            };

            _usuarioRepositoryMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(usuario);

            // Act

            var result = await _service.ObterPorIdAsync(1);

            // Assert

            result.IsSuccess.Should().BeTrue();

            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task AtualizarAsync_QuandoDadosValidos_DeveRetornarSucesso()
        {
            // Arrange

            var dto = new UsuarioDTO
            {
                Nome = "João",
                Email = "joao@email.com",
                Senha = "1234",
                Perfil = new PerfilUsuarioDTO { Id = 2 }
            };

            _passwordHasherMock.Setup(x => x.HashPassword("1234")).Returns("HASH");

            // Act

            var result = await _service.AtualizarAsync(2, dto);

            // Assert

            result.IsSuccess.Should().BeTrue();

            _usuarioRepositoryMock.Verify(x => x.AtualizarAsync(It.Is<Usuario>(u => u.Id == 2 && u.Senha == "HASH")), Times.Once);
        }

        [Fact]
        public async Task FazerLoginAsync_QuandoCredenciaisValidas_DeveRetornarToken()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Email = "admin@email.com",
                Senha = "HASH"
            };

            _usuarioRepositoryMock.Setup(x => x.FazerLogin(It.IsAny<Usuario>())).ReturnsAsync(usuario);

            _passwordHasherMock.Setup(x => x.VerifyPassword("HASH", "123456")).Returns(true);

            _tokenServiceMock.Setup(x => x.GerarToken(usuario)).Returns("TOKEN123");

            // Act
            var result = await _service.FazerLoginAsync(
                new LoginDTO
                {
                    Email = "admin@email.com",
                    Senha = "123456"
                });

            // Assert
            result.IsSuccess.Should().BeTrue();
          
            result.Data!.Token.Should().Be("TOKEN123");           
        }
    }
}
