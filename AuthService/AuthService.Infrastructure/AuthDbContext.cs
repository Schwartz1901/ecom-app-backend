using AuthService.Domain.Aggregates;
using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuthUser>(auth =>
            {
                auth.ToTable("AuthUsers");
                auth.HasKey(a => a.Id);
                auth.Property(a => a.Id).HasConversion(v => v.Value, id => new AuthId(id));
                auth.Property(a => a.Username).IsRequired();
                auth.Property(a => a.Email).IsRequired();
                auth.Property(a => a.PasswordHash).IsRequired();
                auth.Property(a => a.Role).IsRequired();
                

                auth.OwnsMany(a => a.RefreshTokens, rt =>
                {
                    
                    rt.Property(p => p.Id).HasConversion(v => v.Value, id => new TokenId(id));
                    rt.Property(p => p.AuthId)
                    .HasConversion(id => id.Value, value => new AuthId(value))
                    .HasColumnName("AuthId");
                    rt.WithOwner().HasForeignKey(p => p.AuthId);
                    rt.HasKey(p => p.Id);
                    rt.Property(p => p.Token).IsRequired();
                    rt.Property(p => p.CreatedAt).IsRequired();
                    rt.Property(p => p.ExpiresAt).IsRequired();
                    rt.Property(p => p.IsRevoked).IsRequired();

                    rt.ToTable("RefreshTokens");
                });
            });
        }
    }
}
