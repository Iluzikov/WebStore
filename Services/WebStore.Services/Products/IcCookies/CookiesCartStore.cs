using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Products.IcCookies
{
    public class CookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName;

        public CookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cartName = "WebStore-Cart-" + (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                       ? _httpContextAccessor.HttpContext.User.Identity.Name
                       : ""); ;
        }

        public Cart Cart
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
    }
}
