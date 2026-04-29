using Application.Common;
using Application.DTO.Responses;
using Application.Extensions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Generics;
using Domain.Entities;

namespace Application.Services
{
    public class PerfilUsuarioAppService : IReaderAppService<PerfilUsuarioResponseDTO>
    {
        private readonly IGenericRepository<PerfilUsuario> _perfilUsuarioRepository;
        public PerfilUsuarioAppService(IGenericRepository<PerfilUsuario> perfilUsuarioRepository)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
        }
        public async Task<ApplicationResult<PerfilUsuarioResponseDTO>> ObterPorIdAsync(int id)
        {
            try
            {
                var perfilUsuario = await _perfilUsuarioRepository.ObterPorIdAsync(id);

                if(perfilUsuario == null)
                {
                    return ApplicationResult<PerfilUsuarioResponseDTO>
                       .Failure(ApplicationErrors.PerfilUsuarioNaoEncontrado);
                }

                return ApplicationResult<PerfilUsuarioResponseDTO>
                    .Success(perfilUsuario.MapToResponseDTO());
            }
            catch (Exception)
            {

                return ApplicationResult<PerfilUsuarioResponseDTO>
                   .Failure(ApplicationErrors.ErroInterno);
            }
          
        }

        public async Task<ApplicationResult<ICollection<PerfilUsuarioResponseDTO>>> ObterTodosAsync()
        {
            var perfisUsuarios = await _perfilUsuarioRepository.ObterTodosAsync(); 

            if(perfisUsuarios == null)
            {
                return ApplicationResult<ICollection<PerfilUsuarioResponseDTO>>
                   .Failure(ApplicationErrors.PerfilUsuarioNaoEncontrado);
            }

            return ApplicationResult<ICollection<PerfilUsuarioResponseDTO>>
                .Success([.. perfisUsuarios.Select(p => p.MapToResponseDTO())]);
        }
    }
}
