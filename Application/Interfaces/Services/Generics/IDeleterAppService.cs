using Application.Common;

namespace Application.Interfaces.Services.Generics
{
    public interface IDeleterAppService<T>
    {
        Task<ApplicationResult<T>> RemoverAsync(int id);
    }
}
