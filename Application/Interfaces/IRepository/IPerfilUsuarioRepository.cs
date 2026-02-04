using Domain.Entities;

namespace Application.Interfaces.IRepository
{
    public interface IPerfilUsuarioRepository
    {
        Task<ICollection<PerfilUsuario>> ObterTodosAsync();
    }
}
