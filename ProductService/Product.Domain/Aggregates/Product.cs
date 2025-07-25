using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Aggregates
{
    internal class Product : Entity, IAggregateRoot
    {
        public ProductId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Price Price { get; set; }
        public Image Image { get; set; }

        public Product(ProductId id, string name, string description, Price price, Image image) 
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Image = image;
        }

        public void CreateProduct(Product product)
        {

        }

    }
}
