using Cart.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure
{
    public class CartDbContext : DbContext
    {
        public DbSet<CartAggregate> Carts { get; set; }

        public CartDbContext(DbContextOptions<CartDbContext> obtions) : base(obtions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartAggregate>(builder =>
            {
                builder.ToTable("Carts");
                builder.Property(p => p.CartUserId).HasConversion
                (
                    id => id.Value,
                    value => new CartUserId(value)
                ).HasColumnName("Id").IsRequired();
                builder.HasKey(c => c.CartUserId);

                builder.Ignore(c => c.CartItems);
                builder.OwnsMany(c => c.CartItems, item =>
                {
                    item.WithOwner().HasForeignKey("CartId"); // Shadow FK
                    item.Property<Guid>("Id"); // Shadow key if needed
                    item.HasKey("Id"); // EF needs PK even for owned types

                    item.Property(p => p.Name).IsRequired();
                    item.Property(p => p.ImageUrl).IsRequired();
                    item.Property(p => p.ImageAlt).IsRequired();
                    item.Property(p => p.NormalPrice).IsRequired();
                    item.Property(p => p.DiscountPrice).IsRequired();
                    item.Property(p => p.Discount).IsRequired();
                    item.Property(p => p.Quantity).IsRequired();
                    item.Property(p => p.ProductId).IsRequired();

                    item.ToTable("CartItems");
                });
            });
           
        }
    }
}
