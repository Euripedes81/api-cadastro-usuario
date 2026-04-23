using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EfRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly UsuarioDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(UsuarioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<ICollection<T>> ObterTodosAsync()
            => await _dbSet.ToListAsync();

        public virtual async Task<T?> ObterPorIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AdicionarAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var entity = await ObterPorIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }     
    }
}
