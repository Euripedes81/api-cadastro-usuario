namespace Application.Interfaces.IServices.Generics
{
    public interface IReaderAppService<T> where T : class
    {
        Task<ICollection<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
    }
}
