using Cart.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Aggregates.ValueObjects
{
    public class CartItem : ValueObject
    {
        public Guid ProductId { get; set; }
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public string ImageAlt { get; private set; }
        public double NormalPrice { get; private set; }
        public double DiscountPrice { get; private set; }
        public bool Discount { get; private set; }
        public int Quantity { get; private set; }


        private CartItem() { }  
        public CartItem(string name, string imageUrl, string imageAlt, double normalPrice, double discountprice, bool discount, int quantity, Guid productId)
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductId;
            yield return Name;
            yield return ImageUrl;
            yield return ImageAlt;
            yield return NormalPrice;
            yield return DiscountPrice;
            yield return Discount;
            yield return Quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Quantity += quantity;
        }

        public double GetCurrentPrice()
        {
            var unitPrice = Discount ? DiscountPrice : NormalPrice;
            return unitPrice * Quantity;
        }

        

    }
}
