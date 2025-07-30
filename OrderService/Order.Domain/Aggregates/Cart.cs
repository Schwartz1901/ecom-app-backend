using Order.Domain.Aggregates.Entities;
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
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;
        public Guid UserId { get; private set; }

        private Cart() { }
        public Cart(Guid userId)
        {
            UserId = userId;
        }

        public void AddItem(CartItem item) 
        { 
            var exists = _cartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (exists != null)
            {
                exists.IncreaseQuantity(item.Quantity);
            }
            else
            {
                _cartItems.Add(item);
            }
        }
    }
}
