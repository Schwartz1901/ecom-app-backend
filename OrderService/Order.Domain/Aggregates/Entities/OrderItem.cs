using Order.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates.Entities
{
    public class OrderItem : Entity
    {
        public Guid ProductId { get; set; }
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public string ImageAlt { get; private set; }
        public double NormalPrice { get; private set; }
        public double DiscountPrice { get; private set; }
        public bool Discount { get; private set; }
        public int Quantity { get; private set; }
        
        private OrderItem() { }

        public OrderItem(string name, string imageUrl, string imageAlt, double normalPrice, double discountprice, bool discount, int quantity, Guid productId)
        {
            if (Quantity < 0) throw new ArgumentException("Invalid number of units");

            Name = name;
            ImageUrl = imageUrl;
            ImageAlt = imageAlt;
            NormalPrice = normalPrice;
            DiscountPrice = discountprice;
            Discount = discount;
            Quantity = quantity;
            ProductId = productId;
        }

        public void AddQuantity(int num)
        {
            Quantity += num;
        }

        public double GetUnitPrice()
        {
            return Discount ? DiscountPrice : NormalPrice;
        }

        public double GetTotalPrice()
        {
            return (Discount ? DiscountPrice : NormalPrice) * Quantity;
        }
    }
}
