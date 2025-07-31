using Cart.Domain.SeedWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Aggregates.ValueObjects
{
    public class CartUserId : ValueObject
    {
        public Guid Value { get; set; }

        private CartUserId() { }
        public CartUserId(Guid value) { Value = value; }
        public static CartUserId NewId() => new CartUserId(Guid.NewGuid());
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
