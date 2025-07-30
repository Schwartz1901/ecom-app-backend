using Order.Domain.Aggregates.Entities;
using Order.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates
{
    public class Cart : Entity, IAggregateRoot
    {
        public BuyerId BuyerId { get; private set; }
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;

        private Cart() { }
        public Cart(Guid userId)
        {
            UserId = userId;
        }

        public void AddItem(CartItem item) 
        { 
            var exists = _cartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (exists is not null)
            {
                exists.IncreaseQuantity(item.Quantity);
            }
            else
            {
                _cartItems.Add(item);
            }
        }

        public void RemoveItem(CartItem item)
        {
            _cartItems.Remove(item);
        }
        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
