using Application.Common;

namespace Application.Interfaces.IServices.Generics
{
    public interface IDeleterAppService<T>
    {
        Task<ApplicationResult<T>> RemoverAsync(int id);
    }
}
