using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Order.Domain.SeedWork;
using Order.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;
        public EfUnitOfWork(OrderDbContext dbContext) 
        { 
            _dbContext = dbContext; 
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction is not null)
            {
                await _dbContext.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        public async Task RollbackAsync()
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync() ;
                _currentTransaction= null ;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellaionToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellaionToken);
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
        }

    }
}
