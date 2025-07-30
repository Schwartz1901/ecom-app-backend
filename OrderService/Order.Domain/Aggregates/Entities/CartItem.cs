using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates.Entities
{
    public class CartItem : Entity
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public double NormalPrice { get; private set; }
        public double DiscountPrice { get; private set; }
        public bool Discount { get; private set; }
        public int Quantity { get; private set; }


        private CartItem() { }  
        public CartItem(Guid productId, string name, double normalPrice, double discountPrice, bool discount, int quantity)
        {
            ProductId = productId;
            Name = name;
            NormalPrice = normalPrice;
            DiscountPrice = discountPrice;
            Discount = discount;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Quantity += quantity;
        }

        public double GetCurrentPrice()
        {
            return (Discount ? DiscountPrice : NormalPrice) * Quantity;
        }

    }
}
