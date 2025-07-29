using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace Product.Domain.Aggregates.ValueObjects
{
    public class ProductId : ValueObject
    {
        public Guid Id { get; private set; }

        
        public ProductId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.", nameof(id));
            Id = id;
        }

        // Required by EF Core
        
        private ProductId() { }
        

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString() => Id.ToString();
    }
}
