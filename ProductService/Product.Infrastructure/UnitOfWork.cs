using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Product.Domain.SeedWork;
using Product.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;
        public UnitOfWork(ProductDbContext dbContext, IDbContextTransaction currentTransaction) 
        {
            _dbContext = dbContext;
            _currentTransaction = currentTransaction;
        }

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
                await _currentTransaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public void Dispose() => _dbContext.Dispose();
        
    }
}
