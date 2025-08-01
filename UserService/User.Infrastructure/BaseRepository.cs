using Microsoft.EntityFrameworkCore;
using User.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Infrastructure.Repositories
{
    public abstract class BaseRepository<T, TId> : IRepository<T, TId> where T : class, IAggregateRoot
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(TId id)
        {
            return await _dbSet.FindAsync(id);
            
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T t)
        {
            await _dbSet.AddAsync(t);
        }

        public virtual async Task RemoveAsync(TId id)
        {
            var t = await _dbSet.FindAsync(id);
            if (t is not null)
            {
                _dbSet.Remove(t);
            }
        }

        public virtual async Task<bool> UpdateAsync(T t)
        {
            _dbSet.Update(t);
            return await Task.FromResult(true);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
