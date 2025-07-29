using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Product.Domain.Aggregates;
using Product.Domain.Aggregates.Enumerations;
using Product.Domain.Aggregates.ValueObjects;
namespace Product.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<ProductAggregate> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.Entity<ProductAggregate>(builder =>
            {
                builder.Property(p => p.Id)
                   .HasConversion(
                       id => id.Id,              // write to DB
                       value => new ProductId(value) // read from DB
                   )
                   .HasColumnName("Id")
                   .IsRequired();
                    builder.HasKey(p => p.Id);
                //builder.OwnsOne(p => p.Id, id =>
                //{
                //    id.WithOwner();
                //    id.Property(x => x.Id)
                //        .HasColumnName("Id")
                //        .IsRequired()
                //        .ValueGeneratedNever();
                //});
                   
                builder.OwnsOne(p => p.Price);
                builder.OwnsOne(p => p.Image);
                builder.HasMany(p => p.Categories)
                        .WithMany()
                        .UsingEntity(j => j.ToTable("Categories"));

            });

            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedNever();
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Category>().HasData(Category.GetAll<Category>().ToArray());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AttachEnumerations();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AttachEnumerations();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AttachEnumerations()
        {
            foreach (var entry in ChangeTracker.Entries<Category>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Unchanged; // 
                }
            }
        }
    }
}
