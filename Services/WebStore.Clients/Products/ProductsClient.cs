using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductService
    {
        public ProductsClient(IConfiguration configuration) : base(configuration, WebAPI.Products) { }

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_serviceAddress}/brands/{id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{_serviceAddress}/brands");

        public IEnumerable<CategoryDTO> GetCategories() => Get<IEnumerable<CategoryDTO>>($"{_serviceAddress}/categories");

        public CategoryDTO GetCategoryById(int id) => Get<CategoryDTO>($"{_serviceAddress}/categories/{id}");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_serviceAddress}/{id}");

        public PageProductsDTO GetProducts(ProductFilter filter = null) =>
            Post(_serviceAddress, filter ?? new ProductFilter())
            .Content
            .ReadAsAsync<PageProductsDTO>()
            .Result;
    }
}
