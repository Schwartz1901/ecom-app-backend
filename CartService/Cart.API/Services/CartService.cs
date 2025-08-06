using Cart.API.DTOs;
using Cart.API.Interfaces;
using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;
using Cart.Domain.SeedWork;
using MediatR;
using Cart.Domain.Aggregates;
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
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null)
            {
                cart = new CartAggregate(cartId);
                await _cartRepository.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }
            var cartDto = new CartDto
            {
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    Id = item.ProductId,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    ImageAlt = item.ImageAlt,
                    Price = item.NormalPrice,
                    DiscountPrice = item.DiscountPrice,
                    IsDiscount = item.Discount,
                    Name = item.Name,
                    TotalPrice = item.GetCurrentPrice(),
                }).ToList(),
                CartPrice = cart.GetTotalPrice(),
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
                cart = new CartAggregate(cartId);
                await _cartRepository.AddAsync(cart);
                
            }
            var client = _httpClientFactory.CreateClient("ProductService");

            var response = await client.GetAsync($"/api/Product/{dto.ProductId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var cartItemDto = await response.Content.ReadFromJsonAsync<CartItemDto>();
            
            var cartItem = new CartItem(cartItemDto!.Name, 
                cartItemDto.ImageUrl, cartItemDto.ImageAlt, 
                cartItemDto.Price, 
                cartItemDto.DiscountPrice, 
                cartItemDto.IsDiscount, 
                dto.Quantity, 
                dto.ProductId);
          
            cart.AddItem(cartItem);
            await _unitOfWork.SaveChangesAsync(); 

        }

        public async Task RemoveItemAsync(string authId, string itemId)
        {
            var cartUserId = new CartUserId(Guid.Parse(authId));
            var cart = await _cartRepository.GetByIdAsync(cartUserId);

            var productId = Guid.Parse(itemId);
            cart.RemoveItem(productId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
