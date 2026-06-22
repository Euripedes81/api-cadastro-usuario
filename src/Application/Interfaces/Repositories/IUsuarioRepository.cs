using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> FazerLogin(Usuario usuario);
       
    }
}