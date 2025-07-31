using Cart.API.DTOs;
using Cart.API.Interfaces;
using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;

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
       
        public async Task UpdateAsync(CartDto cart)
        {

        }
        public async Task DeleteAsync(Guid id)
        {

        }
    }
}
