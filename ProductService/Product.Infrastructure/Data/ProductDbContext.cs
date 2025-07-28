using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Aggregates;
namespace Product.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<ProductAggregate> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
