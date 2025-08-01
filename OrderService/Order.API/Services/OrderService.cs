using Order.API.DTOs;
using Order.API.Interfaces;
using Order.Domain.Aggregates.ValueObjects;
using Order.Domain.Repositories;
using Order.Infrastructure.Repositories;
using Order.Domain.SeedWork;

namespace Order.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork) 
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderDto> GetByIdAsync(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentException("Invalid Id");
            }
            var orderId = new OrderId(id);
            var result = await _orderRepository.GetByIdAsync(orderId);
            if (result == null)
            {
                throw new KeyNotFoundException("Order not found");
            }
            var orderDto = new OrderDto
            {
                OrderStatus = result.OrderStatus.ToString(),
                Description = result.Description,
                OrderDate = result.OrderDate,
                OrderItems = result.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.GetUnitPrice(),
                    Total = i.GetTotalPrice()
                }).ToList()

            };

            return orderDto;
        }
        public async Task<List<OrderDto>> GetAllAsync()
        {
            var results = await _orderRepository.GetAllAsync();
            var orderDtos = results.Select(result => new OrderDto
            {
                OrderStatus = result.OrderStatus.ToString(),
                Description = result.Description,
                OrderDate = result.OrderDate,
                OrderItems = result.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.GetUnitPrice(),
                    Total = i.GetTotalPrice()
                }).ToList()

            }).ToList();

            return orderDtos;
        }
        public async Task AddAsync(OrderDto orderDto)
        {
            // TODO: Add Order with BuyerId and Fetch data from CartService
        }
        public async Task UpdateAsync(OrderDto orderDto)
        {
            // TODO: Update Order
        }
        public async Task DeleteAsync(Guid id)
        {
            // TODO: Delete Order

        }
    }
}
