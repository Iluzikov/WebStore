using Microsoft.Extensions.Configuration;
using System;
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

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{_serviceAddress}/brands");

        public IEnumerable<CategoryDTO> GetCategories() => Get<IEnumerable<CategoryDTO>>($"{_serviceAddress}/categories");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_serviceAddress}/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null) =>
            Post(_serviceAddress, filter ?? new ProductFilter())
            .Content
            .ReadAsAsync<IEnumerable<ProductDTO>>()
            .Result;
    }
}
