using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class ProductDTOMapper
    {
        public static ProductDTO ToDTO(this Product product) => product is null ? null : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand.ToDTO(),
            Category = product.Category.ToDTO(),
        };
        public static Product FromDTO(this ProductDTO product) => product is null ? null : new Product
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            BrandId = product.Brand?.Id,
            Brand = product.Brand.FromDTO(),
            CategoryId = product.Category.Id,
            Category = product.Category.FromDTO(),
        };

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> products) => products.Select(ToDTO);
        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> products) => products.Select(FromDTO);
    }

}
