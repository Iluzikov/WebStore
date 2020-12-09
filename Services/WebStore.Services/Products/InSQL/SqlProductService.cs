using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context) => _context = context;

        public IEnumerable<CategoryDTO> GetCategories() => _context.Categories.AsEnumerable().Select(c => c.ToDTO());
        

        public IEnumerable<BrandDTO> GetBrands() => _context.Brands.Include(b => b.Products).AsEnumerable().Select(b => b.ToDTO());


        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null)
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

            return query.AsEnumerable().ToDTO();

        }

        public ProductDTO GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id)
                .ToDTO();
        }

        public BrandDTO GetBrandById(int id) => _context.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id).ToDTO();

        public CategoryDTO GetCategoryById(int id) => _context.Categories.Find(id).ToDTO();
    }
}
