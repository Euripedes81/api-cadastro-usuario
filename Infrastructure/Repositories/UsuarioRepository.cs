using Application.Interfaces.IRepository;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioDbContext _context;

        public UsuarioRepository(UsuarioDbContext context) 
        {
            _context = context;
        }
      
        public async Task RemoverAsync(Usuario usuario)
        {           
            _context.Remove(usuario!);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios.Include(u => u.PerfilUsuario).ToListAsync();
        }      

        public async Task AdicionarAsync(Usuario usuario)
        {
            _context.Add(usuario);
           await _context.SaveChangesAsync();
        }
        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios.AsNoTracking().Include(u => u.PerfilUsuario).FirstOrDefaultAsync(u => u.Id == id);
        }       

        public async Task<bool> AtualizarAsync(Usuario usuario)
        {
            var usuarioExistente = await ObterPorIdAsync(usuario.Id);

            if (usuarioExistente != null)
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;

        }
        public async Task<Usuario?> FazerLogin(Usuario usuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Senha == usuario.Senha);
        }
    }
}