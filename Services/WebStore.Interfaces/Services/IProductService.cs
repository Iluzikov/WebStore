using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<BrandDTO> GetBrands();
        BrandDTO GetBrandById(int id);
        IEnumerable<CategoryDTO> GetCategories();
        CategoryDTO GetCategoryById(int id);
        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);
        ProductDTO GetProductById(int id);
    }
}
