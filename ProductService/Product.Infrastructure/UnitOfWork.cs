using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork() { }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        public async Task CommitAsync(CancellationToken cancellationToken = default);
        public async Task RollbackAsync(CancellationToken cancellationToken = default);
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
