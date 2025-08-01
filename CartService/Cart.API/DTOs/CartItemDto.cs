namespace Cart.API.DTOs
{
    public class CartItemDto
    {
 
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public double NormalPrice { get; set; }  
        public double DiscountPrice { get; set; }
        public bool Discount {  get; set; }
        public string ProductName { get; set; } 
        public double TotalPrice { get; set; }
    }
}
