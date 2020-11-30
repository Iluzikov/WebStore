using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Products.InSQL
{
    public class SqlProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context) => _context = context;

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Brand> GetBrands() => _context.Brands;


        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand);

            if (filter?.Ids?.Length > 0)
                query = query.Where(p => filter.Ids.Contains(p.Id));
            else
            {
                if (filter?.BrandId != null)
                    query = query.Where(p => p.BrandId.Equals(filter.BrandId));

                if (filter?.CategoryId != null)
                    query = query.Where(p => p.CategoryId.Equals(filter.CategoryId));
            }

            return query;

        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
