using Order.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates.ValueObjects
{
    public class BuyerId: ValueObject
    {
        public Guid Value { get; set; }

        private BuyerId() { }
        public BuyerId(Guid value)
        {
            Value = value;
        }

        public static BuyerId NewId() => new BuyerId(Guid.NewGuid());

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
