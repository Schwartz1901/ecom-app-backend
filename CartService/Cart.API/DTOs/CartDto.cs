namespace Cart.API.DTOs
{
    public class CartDto
    {
        
        public List<CartItemDto> CartItems { get; set; } = new();
        public double CartPrice { get; set; }
    }

    
}
