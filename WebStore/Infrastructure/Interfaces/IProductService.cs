using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Brand> GetBrands();
        IEnumerable<Category> GetCategories();
        //IEnumerable<Product> GetProducts(ProductFilter filter);
    }
}
