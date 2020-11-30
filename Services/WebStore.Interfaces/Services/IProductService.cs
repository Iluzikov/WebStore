using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<BrandDTO> GetBrands();
        IEnumerable<CategoryDTO> GetCategories();
        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);
        ProductDTO GetProductById(int id);
    }
}
