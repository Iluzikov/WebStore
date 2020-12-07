using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.IcCookies
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly ICartStore _cartStore;
       
        public CartService(IProductService productService, ICartStore CartStore )
        {
            _productService = productService;
            _cartStore = CartStore;
        }

        public void AddToCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
                item.Quantity++;
            else
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });

            _cartStore.Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            
            if (item == null) return;

            //if (item.Quantity > 0)
            //    item.Quantity--;
            
            if (item.Quantity == 0)
                cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = _cartStore.Cart;
            cart.Items.Clear();
            _cartStore.Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item == null) return;
            cart.Items.Remove(item);
            _cartStore.Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = _productService.GetProducts(new ProductFilter
            {
                Ids = _cartStore.Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var products_view_models = products.FromDTO().ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = _cartStore.Cart.Items.Select(item => (products_view_models[item.ProductId], item.Quantity))
            };
        }
    }
}
