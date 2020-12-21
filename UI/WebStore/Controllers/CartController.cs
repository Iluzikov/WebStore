using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{

    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService) => _cartService = cartService;


        public IActionResult Details() => View(new OrderDetailsViewModel { Cart = _cartService.TransformCart() });

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }
        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction(nameof(Details));
        }
        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new OrderDetailsViewModel
                {
                    Cart = _cartService.TransformCart(),
                    Order = model
                });

            var create_order_model = new CreateOrderModel
            {
                Order = model,
                Items = _cartService.TransformCart().Items
                   .Select(item => new OrderItemDTO
                   {
                       Id = item.product.Id,
                       Price = item.product.Price,
                       Quantity = item.quantity
                   })
                   .ToList()
            };

            var order = await orderService.CreateOrder(create_order_model, User.Identity.Name);
            _cartService.RemoveAll();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }


        #region WebApi

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult DecrementFromCartAPI(int id)
        {
            _cartService.DecrementFromCart(id);
            return Ok();
        }
        public IActionResult RemoveFromCartAPI(int id)
        {
            _cartService.RemoveFromCart(id);
            return Ok();
        }
        public IActionResult RemoveAllAPI()
        {
            _cartService.RemoveAll();
            return Ok();
        }
        public IActionResult AddToCartAPI(int id)
        {
            _cartService.AddToCart(id);
            return Json(new { id, message = $"Товар с id:{id} добавлен в корзину"});
        }

        #endregion
    }
}
