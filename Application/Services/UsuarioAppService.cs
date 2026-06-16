using Application.DTO.Responses;
using Application.DTO.Create;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Application.Extensions;
using Application.Common;

namespace Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {       
        public readonly ITokenService _tokenService;
        public readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasher;
        public UsuarioAppService(IUsuarioRepository usuarioRepository, ITokenService tokenService, IPasswordHasherService passwordHasher)
        {
            _usuarioRepository = usuarioRepository;          
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApplicationResult<UsuarioResponseDTO>> ObterPorIdAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(id);

                if (usuario == null)
                {
                    return ApplicationResult<UsuarioResponseDTO>
                        .Failure(ApplicationErrors.NotFound);
                }

                return ApplicationResult<UsuarioResponseDTO>
                    .Success(usuario.MapToResponseDTO());
            }
            catch (Exception)
            {
                return ApplicationResult<UsuarioResponseDTO>
                    .Failure(ApplicationErrors.InternalServerError);
            }
        }
        public async Task<ApplicationResult<int>> AtualizarAsync(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = usuarioDTO.MapToEntity();
            usuario.Id = id;
            usuario.Senha = _passwordHasher.HashPassword(usuarioDTO.Senha!);

            try
            {              
                if (usuario.Id == 1)
                {
                    usuario.PerfilUsuarioId = 1;
                    usuario.Inativo = false;
                }

                await _usuarioRepository.AtualizarAsync(usuario);

                return ApplicationResult<int>.Success(usuario.Id);
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Usuario_Email") == true)
                {
                    return ApplicationResult<int>
                        .Failure(ApplicationErrors.Conflict);
                }

                if (ex.Message.Contains("affected 0 row"))
                {
                    return ApplicationResult<int>.Failure(ApplicationErrors.NotFound);
                }
                
                return ApplicationResult<int>
                    .Failure(ApplicationErrors.InternalServerError);
            }       
        }
        public async Task<ApplicationResult<int>> RemoverAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(id);

                if (usuario == null)
                {
                    return ApplicationResult<int>
                        .Failure(ApplicationErrors.NotFound);
                }

                if (usuario.Id > 1)
                {
                    await _usuarioRepository.RemoverAsync(id);

                    return ApplicationResult<int>.Success(usuario.Id);
                }

                return ApplicationResult<int>.Failure(ApplicationErrors.Forbidden);
            }
            catch (Exception)
            {
                return ApplicationResult<int>.Failure(ApplicationErrors.InternalServerError);
            }            
        }

        public async Task<ApplicationResult<ICollection<UsuarioResponseDTO>>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
           
            if (usuarios == null)
            {
                return ApplicationResult<ICollection<UsuarioResponseDTO>>
                    .Failure(ApplicationErrors.NotFound);
            }

            return ApplicationResult<ICollection<UsuarioResponseDTO>>
               .Success([.. usuarios.Select(u => u.MapToResponseDTO())]);                       
        }

        public async Task<ApplicationResult<int>> AdicionarAsync(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = usuarioDTO.MapToEntity();               
                usuario.Senha = _passwordHasher.HashPassword(usuarioDTO.Senha!);

                await _usuarioRepository.AdicionarAsync(usuario);

                return ApplicationResult<int>.Success(usuario.Id);
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Usuario_Email") == true)
                {
                    return ApplicationResult<int>.Failure(ApplicationErrors.Conflict);
                }

                return ApplicationResult<int>.Failure(ApplicationErrors.InternalServerError);
            }
        }
        public async Task<ApplicationResult<LoginResponseDTO>> FazerLoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.FazerLogin(new Usuario
            {
                Email = loginDTO.Email
            });

            if (usuario == null)
            {
                return ApplicationResult<LoginResponseDTO>
                    .Failure(ApplicationErrors.Unauthorized);
            }

            if (!_passwordHasher.VerifyPassword(usuario.Senha!, loginDTO.Senha!))
            {
                return ApplicationResult<LoginResponseDTO>
                    .Failure(ApplicationErrors.Unauthorized);
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
