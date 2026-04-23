using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PerfilUsuarioRepository : IGenericRepository<PerfilUsuario>
    {
        private readonly UsuarioDbContext _context;
        public PerfilUsuarioRepository(UsuarioDbContext context)
        {
            _context = context;
        }

        public Task AdicionarAsync(PerfilUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AtualizarAsync(PerfilUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<PerfilUsuario?> ObterPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<PerfilUsuario>> ObterTodosAsync()
        {
            return await _context.PerfilUsuarios.ToListAsync();
        }

        public Task RemoverAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<PerfilUsuario>.AtualizarAsync(PerfilUsuario entity)
        {
            return AtualizarAsync(entity);
        }
    }
}