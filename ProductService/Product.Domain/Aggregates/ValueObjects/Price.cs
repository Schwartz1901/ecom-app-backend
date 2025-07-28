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
        public float NormalPrice { get; set; }
        public float DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }
        public Price(float normalPrice, float discountPrice, bool isDiscount) 
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

        public float GetPrice()
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
