using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.SeedWork;

namespace User.Domain.Aggregates.ValueObjects
{
    public class UserId : ValueObject
    {
        public Guid Value { get; set; }

        private UserId() { }

        public UserId(Guid value)
        {
            Value = value;
        }

        public static UserId NewId() => new UserId(Guid.NewGuid());

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
