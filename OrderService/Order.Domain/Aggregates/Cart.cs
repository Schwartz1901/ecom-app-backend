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

    }
}
