using Order.Domain.Aggregates.ValueObjects;
using Order.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Aggregates
{
    public class Buyer : Entity, IAggregateRoot
    {
        public BuyerId Id { get; private set; }
        public Address Address { get; private set; }
        public string Name { get; private set; } = string.Empty;

        private Buyer() { }
        public Buyer(BuyerId id, Address address, string name)
        {
            Id = id;
            Address = address;
            Name = name;
        }
    }
}
