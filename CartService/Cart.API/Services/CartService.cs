using Cart.API.DTOs;
using Cart.API.Interfaces;
using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;
using Cart.Domain.SeedWork;
using MediatR;
using Order.Domain.Aggregates;
using System.Net.WebSockets;

namespace Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }
        public async Task<CartDto> GetByIdAsync(Guid id)
        {
            var cartId = new CartUserId(id);
            var result = await _cartRepository.GetByIdAsync(cartId);

            var cartDto = new CartDto
            {
                CartItems = result.CartItems.Select(item => new CartItemDto
                {
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    ImageAlt = item.ImageAlt,
                    NormalPrice = item.NormalPrice,
                    DiscountPrice = item.DiscountPrice,
                    Discount = item.Discount,
                    ProductName = item.Name,
                    TotalPrice = item.GetCurrentPrice(),
                }).ToList(),
                CartPrice = result.GetTotalPrice(),
            };

            return cartDto;
        }
       
        public async Task<bool> CreateAsync(Guid id)
        {
            var newCartId = new CartUserId(id);
            var newCart = new CartAggregate(newCartId);
            await _cartRepository.AddAsync(newCart);
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public Task UpdateAsync(CartDto cart)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(Guid id)
        {
            var cartId = new CartUserId(id);
            await _cartRepository.RemoveAsync(cartId);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task AddItemToCartAsync(string authId, AddCartItemDto dto)
        {
            var cartId = new CartUserId(Guid.Parse(authId));
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null) 
            { 
                throw new KeyNotFoundException(nameof(cart));
            }
            var client = _httpClientFactory.CreateClient("ProductService");

            var response = await client.GetAsync($"/Product/{dto.ProductId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new KeyNotFoundException($"No Product with Id {dto.ProductId}");
            }

            var cartItemDto = await response.Content.ReadFromJsonAsync<CartItemDto>();
          
            var cartItem = new CartItem(cartItemDto.ProductName, 
                cartItemDto.ImageUrl, cartItemDto.ImageAlt, 
                cartItemDto.NormalPrice, 
                cartItemDto.DiscountPrice, 
                cartItemDto.Discount, 
                dto.Quantity, 
                dto.ProductId);
          
            cart.AddItem(cartItem);
            await _unitOfWork.SaveChangesAsync(); 

        }
    }
}
