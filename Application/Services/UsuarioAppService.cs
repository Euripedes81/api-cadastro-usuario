using Application.DTO.Responses;
using Application.DTO.Create;
using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.Entities;
using Application.Common.Enuns;
using Application.Common.Responses;

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

            return new UsuarioResponseDTO
            {
                Id = id,
                Nome = usuario?.Nome,
                Email = usuario?.Email,              
                Perfil = new PerfilUsuarioResponseDTO { Id = usuario!.PerfilUsuario!.Id, Nome = usuario.PerfilUsuario.Nome },               
                Inativo = usuario.Inativo
            };
        }

        public async Task<AtualizadoDTO> AtualizarAsync(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Id = id,
                Nome = usuarioDTO!.Nome,
                Email = usuarioDTO!.Email,
                Senha = usuarioDTO!.Senha,
                PerfilUsuarioId = usuarioDTO.Perfil!.Id,
                Inativo = usuarioDTO.Inativo
            };

            if (!await _usuarioRepository.AtualizarAsync(usuario))
            {
                return new AtualizadoDTO
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
           
            return new AtualizadoDTO
            {
                Id = usuario!.Id               
            };
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
            return usuarios.Select(u =>
            new UsuarioResponseDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Perfil = new PerfilUsuarioResponseDTO
                {
                    Id = u.PerfilUsuarioId,
                    Nome = u.PerfilUsuario!.Nome
                }
            }).ToList();
        }

        public async Task<UsuarioResponseDTO> AdicionarAsync(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha,
                PerfilUsuarioId = usuarioDTO.Perfil!.Id
            };

            try
            {
                await _usuarioRepository.AdicionarAsync(usuario);
                var usuarioCriado = await _usuarioRepository.ObterPorIdAsync(usuario.Id);

                return new UsuarioResponseDTO
                {
                    Id = usuarioCriado!.Id,
                    Nome = usuarioCriado!.Nome,
                    Email = usuarioCriado.Email,
                    Perfil = new PerfilUsuarioResponseDTO
                    {
                        Id = usuarioCriado.PerfilUsuario!.Id,   
                        Nome = usuarioCriado.PerfilUsuario.Nome
                    }
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

                return new UsuarioResponseDTO
                {
                    Code = code,
                    Mensagem = mensagemException
                };
            }
        }

        public async Task<LoginResponseDTO> FazerLoginAsync(LoginDTO usuarioDTO)
        {
            var usuario = await _usuarioRepository.FazerLogin(new Usuario
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
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
