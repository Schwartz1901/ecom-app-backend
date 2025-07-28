using Product.Domain.Aggregates.Enumerations;
using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Aggregates
{
    public class ProductAggregate : Entity, IAggregateRoot
    {
        public ProductId Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Price Price { get; private set; }
        public Image Image { get; private set; }
        public List<Category> Categories { get; private set; } = new();

        public Product(ProductId id, string name, string description, Price price, Image image, List<Category> categories) 
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Image = image ?? throw new ArgumentNullException(nameof(image));
            Categories = categories?.ToList() ?? throw new ArgumentNullException(nameof(categories));
        }

        

    }
}
