namespace Order.API.DTOs
{
    public class AddOrderDto
    {
        public Guid BuyerId { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }

        public AddressDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public double Total { get; set; }
    }
}
