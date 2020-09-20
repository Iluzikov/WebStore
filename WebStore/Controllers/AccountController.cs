using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOrderService _orderService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOrderService orderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Вход невозможен");
                return View(model);
            }

            if(Url.IsLocalUrl(model.ReturnUrl)) //если ReturnUrl - локальный
                return Redirect(model.ReturnUrl); //перенаправляем туда, откуда пришли

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User { UserName = model.UserName, Email = model.Email };
            var createResult = await _userManager.CreateAsync(user, model.Password);
            
            if (!createResult.Succeeded)
            {
                foreach (var identityError in createResult.Errors) //выводим ошибки
                {
                    ModelState.AddModelError("", identityError.Description);
                    return View(model);
                }
            }
            await _userManager.AddToRoleAsync(user, WebStoreUserRoles.Users);
            await _signInManager.SignInAsync(user, false); //если успешно - логинимся
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult GetOrdersByUser()
        {
            var orders = _orderService.GetUserOrders(User.Identity.Name);
            var userOrder = orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Phone = o.Phone,
                Address = o.Address,
                TotalSum = o.OrderItem.Sum(x => x.Price * x.Quantity)
            });

            return View(userOrder);
        }

    }
}
