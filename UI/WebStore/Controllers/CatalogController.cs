using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
        }

        public IActionResult Shop(int? categoryId, int? brandId, int page = 1, int? pageSize = null)
        {
            var page_size = pageSize 
                ?? (int.TryParse(_configuration["PageSize"], out var size) ? size : (int?)null);

            // получаем список отфильтрованных продуктов
            var filter = new ProductFilter 
            { 
                BrandId = brandId,
                CategoryId = categoryId,
                Page = page,
                PageSize = page_size
            };
            
            var products = _productService.GetProducts(filter);

            return View(new CatalogViewModel
            {
                CategoryId = categoryId,
                BrandId = brandId,
                Products = products.Products.FromDTO().ToView().OrderBy(p => p.Order),
                PageViewModel= new PageViewModel
                {
                    Page = page,
                    PageSize = page_size ?? 0,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound();
            return View(product.FromDTO().ToView());
        }

    }
}
