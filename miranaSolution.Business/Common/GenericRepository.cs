using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Main;

namespace miranaSolution.Business.Common
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MiranaDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(MiranaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdEntity(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}