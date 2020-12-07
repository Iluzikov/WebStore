using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Products.IcCookies;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductService> _productServiceMock;
        private Mock<ICartStore> _cartStoreMock;
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

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);

            _cartService = new CartService(_productServiceMock.Object, _cartStoreMock.Object);

        }

        [TestMethod]
        public void Cart_Class_ItemsCount_returns_Correct_Quantity()
        {
            var cart = _cart;

            const int expected_count = 4;

            var actual_count = cart.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var cart_view_model = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
                }
            };

            const int expected_count = 4;

            var actual_count = cart_view_model.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }


        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            _cart.Items.Clear();

            const int expected_id = 5;
            const int expected_items_count = 1;

            _cartService.AddToCart(expected_id);

            Assert.Equal(expected_items_count, _cart.ItemsCount);

            Assert.Single(_cart.Items);

            Assert.Equal(expected_id, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_RemoveFromCart_Remove_Correct_Item()
        {
            const int item_id = 1;
            const int expected_product_id = 2;

            _cartService.RemoveFromCart(item_id);

            Assert.Single(_cart.Items);

            Assert.Equal(expected_product_id, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartService_Clear_ClearCart()
        {
            _cartService.RemoveAll();

            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int item_id = 2;
            const int expected_quantity = 2;
            const int expected_items_count = 3;
            const int expected_products_count = 2;

            _cartService.DecrementFromCart(item_id);

            Assert.Equal(expected_items_count, _cart.ItemsCount);
            Assert.Equal(expected_products_count, _cart.Items.Count);
            var items = _cart.Items.ToArray();
            Assert.Equal(item_id, items[1].ProductId);
            Assert.Equal(expected_quantity, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;
            const int expected_items_count = 3;

            _cartService.DecrementFromCart(item_id);

            Assert.Equal(expected_items_count, _cart.ItemsCount);
            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartService_TransformFromCart_WorkCorrect()
        {
            const int expected_items_count = 4;
            const decimal expected_first_product_price = 1.1m;

            var result = _cartService.TransformCart();

            Assert.Equal(expected_items_count, result.ItemsCount);
            Assert.Equal(expected_first_product_price, result.Items.First().product.Price);
        }
    }
}
