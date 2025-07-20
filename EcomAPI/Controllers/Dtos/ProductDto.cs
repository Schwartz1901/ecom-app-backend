using System.ComponentModel.DataAnnotations;

namespace EcomAPI.Controllers.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public List<string> Catagory { get; set; } = new();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountPrice { get; set; }

        public bool IsDiscount { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageAlt { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
