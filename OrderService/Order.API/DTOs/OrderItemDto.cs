namespace Order.API.DTOs
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
    }
}
