using Product.Domain.Aggregates.Enumerations;
using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private ProductAggregate() { }
        public ProductAggregate(ProductId id, string name, string description, Price price, Image image, List<Category> categories) 
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Image = image ?? throw new ArgumentNullException(nameof(image));
            Categories = categories?.ToList() ?? throw new ArgumentNullException(nameof(categories));
        }
        public static ProductAggregate Create(string name, string desc, double normalPrice, double discountPrice, bool discount, string imgUrl, string imgAlt, List<string> categoryNames)
        {
            var id = new ProductId(Guid.NewGuid());
            var priceVo = new Price(normalPrice, discountPrice, discount);
            var image = new Image(imgUrl, imgAlt);
            var categories = categoryNames.Select(name => Enumeration.FromName<Category>(name)).ToList();

            return new ProductAggregate(id, name, desc, priceVo, image, categories);
        }
        public void ApplyDiscount(bool isDiscount, double percentage = 0)
        {
            if (isDiscount)
            {
                var discountedPrice = Price.NormalPrice * (1.0 - percentage);
                Price = new Price(Price.NormalPrice, discountedPrice, true);
            }
            else
            {
                Price = new Price(Price.NormalPrice, Price.NormalPrice, false);
            }

        }

        public void UpdateName(string name) => Name = name;
        public void UpdateDescription(string description) => Description = description;
        public void UpdatePrice(Price price) => Price = price;
        public void SetImage(Image image) => Image = image;
    
        public void SetCategories(List<Category> categories) => Categories = categories;



    }
}
