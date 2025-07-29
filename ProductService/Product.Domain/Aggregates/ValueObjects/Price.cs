using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Aggregates.ValueObjects
{
    public class Price : ValueObject
    {
        public double NormalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }
        public Price(double normalPrice, double discountPrice, bool isDiscount) 
        {
            NormalPrice = normalPrice;
            DiscountPrice = discountPrice;
            IsDiscount = isDiscount;

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NormalPrice;
            yield return DiscountPrice;
            yield return IsDiscount;
        }

        public double GetCurrentPrice()
        {
            if (IsDiscount)
            {
                return DiscountPrice;
            }
            else
            {
                return NormalPrice;
            }
        }


       
    }
}
