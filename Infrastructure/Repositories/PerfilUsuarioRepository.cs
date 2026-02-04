using Application.Interfaces.IRepository;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PerfilUsuarioRepository : IPerfilUsuarioRepository
    {
        private UsuarioDbContext _context;
        public PerfilUsuarioRepository(UsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PerfilUsuario>> ObterTodosAsync()
        {
            return await _context.PerfilUsuarios.ToListAsync();
        }
    }
}