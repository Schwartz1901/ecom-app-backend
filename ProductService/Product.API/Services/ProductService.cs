using Product.API.DTOs;
using Product.API.Interfaces;
using Product.Domain.Aggregates;
using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.Repositories;
using Product.Domain.SeedWork;

namespace Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        


        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var productId = new ProductId(id);

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            var productDto = new ProductDto
            {
                Id = product.Id.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.NormalPrice,
                DiscountPrice = product.Price.DiscountPrice,
                IsDiscount = product.Price.IsDiscount,
                ImageUrl = product.Image.ImageUrl,
                ImageAlt = product.Image.ImageAlt,
                Categories = product.Categories.Select(c => c.Name).ToList()


            };

           

            return productDto;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            List<ProductDto> result = new List<ProductDto>();
            foreach(var product in products)
            {
                var dto = new ProductDto
                {
                    Id = product.Id.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.NormalPrice,
                    DiscountPrice = product.Price.DiscountPrice,
                    IsDiscount = product.Price.IsDiscount,
                    ImageUrl = product.Image.ImageUrl,
                    ImageAlt = product.Image.ImageAlt,
                    Categories = product.Categories.Select(c => c.Name).ToList()
                };
                result.Add(dto);
            }

            return result;
        }

        public async Task AddAsync(ProductDto dto)
        {

            var productAggregate = ProductAggregate.Create(

                dto.Name,
                dto.Description,
                dto.Price,
                dto.ImageUrl,
                dto.ImageAlt,
                dto.Categories
                );

            await _productRepository.AddAsync(productAggregate);

        }

        public async Task DeleteAsync(Guid id)
        {
            var productId = new ProductId(id);
            await _productRepository.RemoveAsync(productId);
        }
 
    }
}
