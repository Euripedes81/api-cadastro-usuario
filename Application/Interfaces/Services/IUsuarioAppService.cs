using Application.Common;
using Application.DTO.Create;
using Application.DTO.Responses;
using Application.Interfaces.Services.Generics;

namespace Application.Interfaces.Services
{
    public interface IUsuarioAppService : IReaderAppService<UsuarioResponseDTO>, IDeleterAppService
    {
        Task<ApplicationResult<LoginResponseDTO>> FazerLoginAsync(LoginDTO loginDTO);
        Task<ApplicationResult<int>> AtualizarAsync(int id, UsuarioDTO usuarioDTO);
        Task<ApplicationResult<int>> AdicionarAsync(UsuarioDTO usuarioDTO);
    }
}
