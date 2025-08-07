using Order.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates.ValueObjects
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string Ward { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }

        public Address() { }

        public Address(string street, string city, string ward, string country, string zipcode)
        {
            Street = street;
            City = city;
            Ward = ward;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return Ward;
            yield return Country;
            yield return ZipCode;
        }
    }
}
