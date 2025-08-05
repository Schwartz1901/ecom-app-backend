using AuthService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Aggregates.ValueObjects
{
    public class AuthId : ValueObject
    {
        public Guid Value { get; private set; }

        private AuthId() { }
        public AuthId(Guid value)
        {
            Value = value;
        }

        public static AuthId New() => new AuthId(Guid.NewGuid());

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
