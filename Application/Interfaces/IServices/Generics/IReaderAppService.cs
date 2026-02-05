using Application.DTO.Responses;
using Application.Services;

namespace Application.Interfaces.IServices.Generics
{
    public interface IReaderAppService<T> where T : class
    {
        Task<ICollection<T>> ObterTodosAsync();
        Task<ApplicationResult<T>> ObterPorIdAsync(int id);
    }
}
