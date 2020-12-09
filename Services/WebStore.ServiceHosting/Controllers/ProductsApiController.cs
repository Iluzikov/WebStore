using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>API Управления продуктами</summary>
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductService
    {
        private readonly IProductService _productService;
        public ProductsApiController(IProductService productService) => _productService = productService;

        
        /// <summary>Получение списка брендов</summary>
        /// <returns>Список брендов</returns>
        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => _productService.GetBrands();

        /// <summary>Получение бренда по идентификатору</summary>
        /// <returns>Бренд</returns>
        [HttpGet("brands/{id}")]
        public BrandDTO GetBrandById(int id) => _productService.GetBrandById(id);

        /// <summary>Получение списка категорий</summary>
        /// <returns>Список категорий</returns>
        [HttpGet("categories")]
        public IEnumerable<CategoryDTO> GetCategories() => _productService.GetCategories();

        /// <summary>Получение категории по идентификатору</summary>
        /// <returns>Категория</returns>
        [HttpGet("categories/{id}")]
        public CategoryDTO GetCategoryById(int id) => _productService.GetCategoryById(id);

        /// <summary>Получение продукта по идентификатору</summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns>Продукт</returns>
        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => _productService.GetProductById(id);

        /// <summary>Получение отфильтрованного списка продуктов</summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Отфильтрованный список продуктов</returns>
        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody]ProductFilter filter = null) =>
            _productService.GetProducts(filter ?? new ProductFilter());
    }
}
