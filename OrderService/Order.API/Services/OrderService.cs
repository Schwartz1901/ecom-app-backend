using Order.API.DTOs;
using Order.API.Interfaces;
using Order.Domain.Aggregates.ValueObjects;
using Order.Domain.Repositories;
using Order.Infrastructure.Repositories;
using Order.Domain.SeedWork;
using System.Net.Http.Headers;
using Order.Domain.Aggregates.Entities;
using Order.Domain.Aggregates;
using Order.Domain.Aggregates.Enumerations;

namespace Order.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(
            IOrderRepository orderRepository, 
            IUnitOfWork unitOfWork, 
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor
            ) 
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<List<OrderDto>> GetHistoryAsync(string id)
        {
            var buyerId = new BuyerId(Guid.Parse(id));
            var history = await _orderRepository.GetListByBuyerIdAsync(buyerId);
            var results = history.Select(result => new OrderDto
            { 
                OrderStatus = result.OrderStatus.ToString(),
                Description = result.Description,
                OrderDate = result.OrderDate,
                OrderItems = result.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    ImageAlt = i.ImageAlt,
                    ImageUrl = i.ImageUrl,
                    UnitPrice = i.GetUnitPrice(),
                    Total = i.GetTotalPrice()
                }).ToList(),
                Total = result.GetTotal()
            }
            ).ToList();

            return results;
        }
        public Task AddAsync(OrderDto orderDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(OrderDto orderDto)
        {
            // TODO: Update Order
            throw new NotImplementedException();
        }
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
            // TODO: Delete Order

        }

        public async Task CheckoutAsync(string userId, AddressDto addressDto, string description)
        {
            var cartClient = _httpClientFactory.CreateClient("CartService");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Cart");

            var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var token = jwt?.Replace("Bearer ", "");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var cartResponse = await cartClient.SendAsync(request);
                if (!cartResponse.IsSuccessStatusCode)
                {
                    var error = cartResponse.Content.ReadAsStringAsync();
                    throw new Exception("Error at CartService: " + error);

                }
                var cart = await cartResponse.Content.ReadFromJsonAsync<CartDto>();
                var address = new Address(
                    addressDto.Street, 
                    addressDto.City, 
                    addressDto.Ward, 
                    addressDto.Country, 
                    addressDto.ZipCode);

                var orderItems = cart.CartItems.Select(i => new OrderItem
                    (
                        i.Name,
                        i.ImageUrl,
                        i.ImageAlt,
                        i.Price,
                        i.DiscountPrice,
                        i.IsDiscount,
                        i.Quantity,
                        i.Id
                    )
                ).ToList();

                var order = new OrderAggregate(new BuyerId(Guid.Parse(userId)), address, DateTime.UtcNow, description, OrderStatus.Submitted, orderItems);
                await _orderRepository.AddAsync(order);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
