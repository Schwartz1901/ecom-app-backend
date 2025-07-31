using Cart.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Aggregates;
using Order.Domain.Aggregates.Entities;
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
                builder.Navigation(c => c.CartItems)
                    .UsePropertyAccessMode(PropertyAccessMode.Field);

                builder
                    .HasMany(typeof(CartItem), "_cartItems")
                    .WithOne()
                    .HasForeignKey("CartId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
