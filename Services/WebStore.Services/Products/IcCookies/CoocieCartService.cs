using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.IcCookies
{
    public class CoocieCartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName;
        private Cart Cart
        {
            get
            {
                var cookie = _httpContextAccessor
                                .HttpContext
                                .Request
                                .Cookies[_cartName];
                string json = string.Empty;
                Cart cart = null;

                if (cookie == null)
                {
                    cart = new Cart { Items = new List<CartItem>() };
                    json = JsonConvert.SerializeObject(cart);

                    _httpContextAccessor
                          .HttpContext
                          .Response
                          .Cookies
                          .Append(
                             _cartName,
                             json,
                             new CookieOptions
                             {
                                 Expires = DateTime.Now.AddDays(1)
                             });
                    return cart;
                }

                json = cookie;
                cart = JsonConvert.DeserializeObject<Cart>(json);

                _httpContextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Delete(_cartName);

                _httpContextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Append(
                          _cartName,
                          json,
                          new CookieOptions()
                          {
                              Expires = DateTime.Now.AddDays(1)
                          });

                return cart;
            }

            set
            {
                var json = JsonConvert.SerializeObject(value);

                _httpContextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Delete(_cartName);
                _httpContextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Append(
                           _cartName,
                           json,
                           new CookieOptions()
                           {
                               Expires = DateTime.Now.AddDays(1)
                           });
            }
        }

        public CoocieCartService(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _cartName = "cart" + (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                       ? _httpContextAccessor.HttpContext.User.Identity.Name
                       : ""); ;
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
                item.Quantity++;
            else
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
                return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveAll()
        {
            Cart = new Cart { Items = new List<CartItem>() };
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item == null) return;
            cart.Items.Remove(item);
            Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = _productService.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var products_view_models = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items.Select(item => (products_view_models[item.ProductId], item.Quantity))
            };
        }
    }
}
