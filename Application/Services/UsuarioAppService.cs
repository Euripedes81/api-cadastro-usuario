using Application.DTO.Responses;
using Application.DTO.Create;
using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.Entities;
using Application.Extensions;
using System.Net;

namespace Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        public readonly ITokenService _tokenService;
        public readonly IUsuarioRepository _usuarioRepository;
        public UsuarioAppService(IUsuarioRepository usuarioRepository, IPerfilUsuarioRepository perfilUsuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<ApplicationResult<UsuarioResponseDTO>> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario == null)
            {
                return ApplicationResult<UsuarioResponseDTO>
                    .Failure(ApplicationErrors.UsuarioNaoEncontrado);
            }

            return ApplicationResult<UsuarioResponseDTO>
                .Success(usuario.MapToResponseDTO());
        }
        public async Task<ApplicationResult<int>> AtualizarAsync(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = usuarioDTO.MapToEntity();
            usuario.Id = id;

            try
            {
                var atualizado = await _usuarioRepository.AtualizarAsync(usuario);

                if (!atualizado)
                {
                    return ApplicationResult<int>
                        .Failure(ApplicationErrors.UsuarioNaoEncontrado);
                }

                // regra de negócio continua aqui
                if (usuario.Id == 1)
                {
                    usuario.PerfilUsuarioId = 1;
                    usuario.Inativo = false;
                }

                return ApplicationResult<int>.Success(usuario.Id);
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Usuario_Email") == true)
                {
                    return ApplicationResult<int>
                        .Failure(ApplicationErrors.EmailJaExiste);
                }

                return ApplicationResult<int>
                    .Failure(ApplicationErrors.ErroInterno);
            }
        }
        public async Task RemoverAsync(int id)
        {
            var usuario = _usuarioRepository.ObterPorIdAsync(id);

            if (usuario?.Id > 1)
            {
                await _usuarioRepository.RemoverAsync(id);
            }
        }

        public async Task<ApplicationResult<ICollection<UsuarioResponseDTO>>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
           
            if (usuarios == null)
            {
                return ApplicationResult<ICollection<UsuarioResponseDTO>>
                    .Failure(ApplicationErrors.UsuarioNaoEncontrado);
            }

            return ApplicationResult<ICollection<UsuarioResponseDTO>>
               .Success(usuarios.Select(u => u.MapToResponseDTO()).ToList());                       
        }

        public async Task<ApplicationResult<int>> AdicionarAsync(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = usuarioDTO.MapToEntity();
                await _usuarioRepository.AdicionarAsync(usuario);

                return ApplicationResult<int>.Success(usuario.Id);
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Usuario_Email") == true)
                {
                    return ApplicationResult<int>.Failure(ApplicationErrors.EmailJaExiste);
                }

                return ApplicationResult<int>.Failure(ApplicationErrors.ErroInterno);
            }
        }
        public async Task<ApplicationResult<LoginResponseDTO>> FazerLoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.FazerLogin(new Usuario
            {
                Email = loginDTO.Email,
                Senha = loginDTO.Senha
            });

            if (usuario == null)
            {
                return ApplicationResult<LoginResponseDTO>
                    .Failure(ApplicationErrors.CredenciaisInvalidas);
            }

            var token = _tokenService.GerarToken(usuario);

            return ApplicationResult<LoginResponseDTO>.Success(new LoginResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = token
            });
        }
    }
}
