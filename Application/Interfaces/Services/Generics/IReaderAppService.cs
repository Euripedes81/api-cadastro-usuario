using Application.Common;
using Application.DTO.Responses;

namespace Application.Interfaces.Services.Generics
{
    public interface IReaderAppService<T> where T : class
    {
        Task<ApplicationResult<ICollection<UsuarioResponseDTO>>> ObterTodosAsync();
        Task<ApplicationResult<T>> ObterPorIdAsync(int id);
    }
}
