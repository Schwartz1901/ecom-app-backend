using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Aggregates;
using User.Domain.Aggregates.ValueObjects;

namespace User.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserAggregate> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAggregate>(builder =>
            {
                builder.ToTable("Users");

                builder.HasKey(u => u.Id);
                
                builder.Property(u => u.Id)
                    .HasConversion(id => id.Value, value => new UserId(value))
                    .HasColumnName("Id")
                    .IsRequired();
                builder.Property(u => u.AuthId).IsRequired();
                builder.Property(u => u.Username).IsRequired();
                builder.Property(u => u.Email).IsRequired();
                builder.Property(u => u.PhoneNumber);
                builder.Property(u => u.CreatedAt);
                builder.Property(u => u.IsActive);


            });
         }
    }
}
