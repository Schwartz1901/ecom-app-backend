using Cart.API.DTOs;
using Cart.API.Interfaces;
using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;
using Order.Domain.Aggregates;

namespace Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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

        public async Task UpdateAsync(CartDto cart)
        {

        }
        public async Task DeleteAsync(Guid id)
        {
            var cartId = new CartUserId(id);
            await _cartRepository.RemoveAsync(cartId);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
