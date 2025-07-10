using Microsoft.EntityFrameworkCore;

namespace DocumentAPI.Models
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) 
        { 
        }
        public DbSet<ProductEntity> ProductEntities { get; set; } = null;
    }
}
