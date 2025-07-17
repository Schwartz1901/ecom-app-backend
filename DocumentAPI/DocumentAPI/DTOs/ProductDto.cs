using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage ="Name is required")]
        [MinLength(3, ErrorMessage ="Name must be at least 3 characters")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        public float Price { get; set; }
        
    }
}
