using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Products.IcCookies;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Products
{
    [TestClass]
    class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductService> _productServiceMock;
        private ICartService _cartService;


        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new() { ProductId = 1, Quantity = 1 },
                    new() { ProductId = 2, Quantity = 3 },
                }
            };

            _productServiceMock = new Mock<IProductService>();
            _productServiceMock
                .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new List<ProductDTO>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO { Id = 1, Name = "Brand 1" },
                        Category = new CategoryDTO { Id = 1, Name = "Category 1"}
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDTO { Id = 2, Name = "Brand 2" },
                        Category = new CategoryDTO { Id = 2, Name = "Category 2"}
                    },
                });


            ///_cartService = new CoocieCartService(_productServiceMock.Object, );
            
        }

        [TestMethod]
        public void Cart_Class_ItemsCount_returns_Correct_Quantity()
        {


        }


    }
}
