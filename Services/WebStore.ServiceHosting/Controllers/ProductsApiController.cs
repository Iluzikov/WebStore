using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductService
    {
        private readonly IProductService _productService;
        public ProductsApiController(IProductService productService) => _productService = productService;

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => _productService.GetBrands();

        [HttpGet("categories")]
        public IEnumerable<CategoryDTO> GetCategories() => _productService.GetCategories();

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => _productService.GetProductById(id);

        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody]ProductFilter filter = null) =>
            _productService.GetProducts(filter ?? new ProductFilter());
    }
}
