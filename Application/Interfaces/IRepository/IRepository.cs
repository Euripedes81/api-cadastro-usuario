namespace Application.Interfaces.IRepository
{
    public interface IRepository<T> where T : class     
    {
        Task<ICollection<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(T obj);
        Task RemoverAsync(int id);
        Task AdicionarAsync(T obj);
    }
}