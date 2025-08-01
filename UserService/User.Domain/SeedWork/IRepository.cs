using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.SeedWork
{
    public interface IRepository<T, TId> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(TId id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task RemoveAsync(TId id);
        Task<bool> UpdateAsync(T entity);
        Task SaveChangesAsync();


    }
}
