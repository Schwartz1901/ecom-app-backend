using Microsoft.EntityFrameworkCore;
using Order.Domain.Aggregates;
using Order.Domain.Aggregates.Enumerations;
using Order.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<OrderAggregate> Orders { get; set; }
  
        public DbSet<Buyer> Buyer { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderAggregate>(builder =>
            {
                builder.ToTable("Orders");
                builder.Property(p => p.Id).HasConversion
                (
                    id => id.Value,
                    value => new OrderId(value)
                ).HasColumnName("Id").IsRequired();
                builder.HasKey(o => o.Id);
                builder.Property(b => b.BuyerId).HasConversion
                (
                    id => id.Value,
                    value => new BuyerId(value)
                ).HasColumnName("Buyer_Id").IsRequired();

                builder.Property(o => o.OrderStatus).HasConversion(
                        os => os.Id,
                        v => OrderStatus.From(v)
                        )
                .HasColumnName("Order_Status_Id")
                .IsRequired();
             

                builder.Property(o => o.OrderDate).HasColumnName("Order_Date").IsRequired();
                builder.Property(o => o.Description).HasColumnName("Description");

                // OwnsType

                builder.OwnsOne(o => o.Address, address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.Ward).HasColumnName("Ward");
                    address.Property(a => a.Country).HasColumnName("Country");
                    address.Property(a => a.ZipCode).HasColumnName("ZipCode");
                });

                // One-to-many

                builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey("OrderId");
                builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);

            });
           

            modelBuilder.Entity<Buyer>(builder =>
            {
                builder.ToTable("Buyers");
                builder.Property(b => b.Id).HasConversion
                (
                    id => id.Value,
                    value => new BuyerId(value)
                ).HasColumnName("Id").IsRequired();

                builder.HasKey(b => b.Id);

                builder.OwnsOne(b => b.Address, address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.Ward).HasColumnName("Ward");
                    address.Property(a => a.Country).HasColumnName("Country");
                    address.Property(a => a.ZipCode).HasColumnName("ZipCode");
                });

                builder.Property(b => b.Name).HasColumnName("Name");
            });
        }
    }
}
