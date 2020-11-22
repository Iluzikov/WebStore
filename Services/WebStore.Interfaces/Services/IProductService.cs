using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<Brand> GetBrands();
        IEnumerable<Category> GetCategories();
        IEnumerable<Product> GetProducts(ProductFilter filter);
        Product GetProductById(int id);
    }
}
