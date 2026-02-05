using Application.DTO.Create;
using Application.DTO.Responses;
using Application.Interfaces.IServices.Generics;
using Application.Services;

namespace Application.Interfaces.IServices
{
    public interface IUsuarioAppService : IReaderAppService<UsuarioResponseDTO>, IDeleterAppService
    {
        Task<LoginResponseDTO> FazerLoginAsync(LoginDTO loginDTO);
        Task<ApplicationResult<int>> AtualizarAsync(int id, UsuarioDTO usuarioDTO);
        Task<ApplicationResult<int>> AdicionarAsync(UsuarioDTO usuarioDTO);
    }
}
