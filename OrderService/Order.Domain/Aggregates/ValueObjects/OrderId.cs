using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates.ValueObjects
{
    public class OrderId : ValueObject
    {
        public Guid Value { get; set; }

        private OrderId() { }

        public OrderId(Guid value)
        {
            Value = value;
        }

        public static OrderId NewId() => new OrderId(Guid.NewGuid());

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
        
    }
}
