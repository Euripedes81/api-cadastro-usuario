using Application.DTO.Responses;
using Application.DTO.Create;
using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.Entities;
using Application.Common.Enuns;
using Application.Common.Responses;
using Application.Extensions;

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
    
        public async Task<UsuarioResponseDTO?> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if(usuario == null)
            {
                return new UsuarioResponseDTO
                {
                    Code = (int)StatusCodeMessage.NotFound,
                    Mensagem = StatusCodeMessage.NotFound.ToString()
                };
            }

            return usuario.MapToResponseDTO();
        }

        public async Task<GenericResponseDTO> AtualizarAsync(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = usuarioDTO.MapToEntity();
            usuario.Id = id;

            try
            {
                if (!await _usuarioRepository.AtualizarAsync(usuario))
                {
                    return new GenericResponseDTO
                    {
                        Code = (int)StatusCodeMessage.NotFound,
                        Mensagem = StatusCodeMessage.NotFound.ToString()
                    };
                }

                if (usuario.Id == 1)
                {
                    usuario.PerfilUsuarioId = 1;
                    usuario.Inativo = false;
                }

                return new GenericResponseDTO
                {
                    Id = usuario!.Id
                };
            }
            catch (Exception ex)
            {
                string mensagemException = StatusCodeMessage.InternalServerError.ToString();
                int code = (int)StatusCodeMessage.InternalServerError;

                if (ex.InnerException!.Message.ToString().Contains("IX_Usuario_Email"))
                {
                    code = (int)StatusCodeMessage.Conflict;
                    mensagemException = $"{StatusCodeMessage.Conflict}: {StatusMessageResponse.EmailJaExiste}";
                }

                return new GenericResponseDTO
                {
                    Code = code,
                    Mensagem = mensagemException
                };
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

        public async Task<ICollection<UsuarioResponseDTO>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return usuarios.Select(u => u.MapToResponseDTO()).ToList();
        }

        public async Task<GenericResponseDTO> AdicionarAsync(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = usuarioDTO.MapToEntity();

            try
            {
                await _usuarioRepository.AdicionarAsync(usuario);
                var usuarioCriado = await _usuarioRepository.ObterPorIdAsync(usuario.Id);

                return new GenericResponseDTO { Id = usuarioCriado!.Id };
            }
            catch (Exception ex)
            {
                string mensagemException = StatusCodeMessage.InternalServerError.ToString();
                int code = (int)StatusCodeMessage.InternalServerError;

                if (ex.InnerException!.Message.ToString().Contains("IX_Usuario_Email"))
                {
                    code = (int)StatusCodeMessage.Conflict;
                    mensagemException = $"{StatusCodeMessage.Conflict}: {StatusMessageResponse.EmailJaExiste}";
                }

                return new GenericResponseDTO
                {
                    Code = code,
                    Mensagem = mensagemException
                };
            }
        }

        public async Task<LoginResponseDTO> FazerLoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.FazerLogin(new Usuario
            {
                Email = loginDTO.Email,
                Senha = loginDTO.Senha
            });

            if (usuario != null)
            {
                string token = _tokenService.GerarToken(usuario);

                return new LoginResponseDTO
                {
                    Id = usuario!.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Token = token
                };
            }

            return new LoginResponseDTO
            {
                Mensagem = StatusCodeMessage.NotFound.ToString(),
                Code = (int)StatusCodeMessage.NotFound
            };

        }
    }
}
