namespace Cart.API.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public double Price { get; set; }  
        public double DiscountPrice { get; set; }
        public bool IsDiscount {  get; set; }
        public string Name { get; set; } 
        public double TotalPrice { get; set; }
    }


    public class AddCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
