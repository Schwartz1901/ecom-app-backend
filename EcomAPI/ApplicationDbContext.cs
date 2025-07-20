using AuthAPI.Models;
using EcomAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthAPI
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var stringListConverter = new ValueConverter<List<string>, string>(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            modelBuilder.Entity<Product>()
                .Property(p => p.Catagory)
                .HasConversion(stringListConverter);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Catagory = new List<string> { "Herbal Tea" },
                    Name = "Chamomile Calm",
                    Price = 6.99m,
                    DiscountPrice = 5.49m,
                    IsDiscount = true,
                    ImageUrl = "https://shop.clipper-teas.com/cdn/shop/products/20-Chamomile-Infusion-NEW.jpg?v=1442224519",
                    ImageAlt = "Chamomile tea box",
                    Description = "A soothing blend of chamomile flowers for relaxation and better sleep."
                },
                new Product
                {
                    Id = 2,
                    Catagory = new List<string> { "Herbal Tea" },
                    Name = "Peppermint Refresh",
                    Price = 5.99m,
                    DiscountPrice = 0,
                    IsDiscount = false,
                    ImageUrl = "https://www.harrisfarm.com.au/cdn/shop/products/mint-refresh-done.jpg?v=1569026289",
                    ImageAlt = "Peppermint tea",
                    Description = "Invigorating peppermint leaves to aid digestion and refresh your senses."
                },
                new Product
                {
                    Id = 3,
                    Catagory = new List<string> { "Herbal Tea", "Detox" },
                    Name = "Lemongrass Ginger Detox",
                    Price = 7.49m,
                    DiscountPrice = 6.49m,
                    IsDiscount = true,
                    ImageUrl = "https://suspire.in/cdn/shop/files/Lemongrassginger-1_large.jpg?v=1716030612",
                    ImageAlt = "Lemongrass and ginger tea",
                    Description = "A spicy and citrusy blend to support cleansing and immunity."
                },
                new Product
                {
                    Id = 4,
                    Catagory = new List<string> { "Herbal Tea" },
                    Name = "Hibiscus Bloom",
                    Price = 6.29m,
                    DiscountPrice = 0,
                    IsDiscount = false,
                    ImageUrl = "https://i.ebayimg.com/images/g/h1IAAeSwG8tofKHM/s-l1600.webp",
                    ImageAlt = "Hibiscus tea",
                    Description = "Tart and vibrant hibiscus petals known for supporting heart health."
                },
                new Product
                {
                    Id = 5,
                    Catagory = new List<string> { "Herbal Tea", "Relaxation" },
                    Name = "Lavender Serenity",
                    Price = 8.49m,
                    DiscountPrice = 7.49m,
                    IsDiscount = true,
                    ImageUrl = "https://images.heb.com/is/image/HEBGrocery/001433383-1",
                    ImageAlt = "Lavender tea",
                    Description = "Floral lavender blend to help reduce stress and promote calm."
                }
            
            );
        }
    }
}
