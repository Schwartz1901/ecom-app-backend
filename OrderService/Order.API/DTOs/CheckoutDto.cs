namespace Order.API.DTOs
{
    public class CheckoutDto
    {
        public string Recipient { get; set; }
        public AddressDto Address { get; set; }
        public string Description { get; set; }

    }
    public class CartDto
    {
        public List<CartItemDto> CartItems { get; set; } = new();
        public double CartPrice { get; set; }
    }
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
    }
}

