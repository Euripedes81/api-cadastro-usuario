using Application.DTO.Responses;
using Application.Services;

namespace Application.Interfaces.IServices.Generics
{
    public interface IReaderAppService<T> where T : class
    {
        Task<ApplicationResult<ICollection<UsuarioResponseDTO>>> ObterTodosAsync();
        Task<ApplicationResult<T>> ObterPorIdAsync(int id);
    }
}
