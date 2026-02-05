using Application.DTO.Responses;
using Application.DTO.Create;
using Application.Interfaces.IServices.Generics;

namespace Application.Interfaces.IServices
{
    public interface IUsuarioAppService : IReaderAppService<UsuarioResponseDTO>, IDeleterAppService
    {
        Task<LoginResponseDTO> FazerLoginAsync(LoginDTO loginDTO);
        Task<GenericResponseDTO> AtualizarAsync(int id, UsuarioDTO usuarioDTO);
        Task<GenericResponseDTO> AdicionarAsync(UsuarioDTO usuarioDTO);
    }
}
