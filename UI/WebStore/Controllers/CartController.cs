﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Controllers
{

    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        
        public CartController(ICartService cartService) => _cartService = cartService;


        public IActionResult Details() => View(new OrderDetailsViewModel { Cart = _cartService.TransformCart()});

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction("Details");
        }
        public IActionResult AddToCart(int id, string returnUrl)
        {
            _cartService.AddToCart(id);
            return Redirect(returnUrl);
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

            var order = await orderService.CreateOrder(model, _cartService.TransformCart(), User.Identity.Name);
            _cartService.RemoveAll();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}