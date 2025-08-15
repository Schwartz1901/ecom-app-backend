namespace Product.API.DTOs
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }

        public List<string> Categories { get; set; } = new();
    }

    public class AddProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }

        public List<string> Categories { get; set; } = new();
    }

    public class UpdateProductDto 
    {
        public string Name { get; set; } = default!;
        public string Categories { get; set; } = default!;
        public double Price { get; set; }
        public bool IsDiscount { get; set; }
        public double DiscountPrice { get; set; }
        public string Description { get; set; } = default!;
        public bool? RemoveImage { get; set; }         
        public IFormFile? Image { get; set; }
    }
}
