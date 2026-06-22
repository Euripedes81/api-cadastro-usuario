namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ICollection<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task AtualizarAsync(T entity);
        Task RemoverAsync(int id);
        Task AdicionarAsync(T entity);
    }
}
