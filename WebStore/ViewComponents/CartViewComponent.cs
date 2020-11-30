using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View(_cartService.TransformCart());
        }
    }
}
