using Domain.Entities;

namespace Application.Interfaces.IRepository
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> FazerLogin(Usuario usuario);
        Task<ICollection<Usuario>> ObterTodosAsync();
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(Usuario usuario);
        Task RemoverAsync(int id);
        Task AdicionarAsync(Usuario usuario);
    }
}