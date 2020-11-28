using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (loginResult.Succeeded)
            {
                if (Url.IsLocalUrl(model.ReturnUrl)) //если ReturnUrl - локальный
                    return Redirect(model.ReturnUrl); //перенаправляем туда, откуда пришли
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { UserName = model.UserName/*, Email = model.Email */};
            var registrationResult = await _userManager.CreateAsync(user, model.Password);

            if (registrationResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.User);
                await _signInManager.SignInAsync(user, false); //если успешно - логинимся
                return RedirectToAction("Index", "Home");
            }

            foreach (var identityError in registrationResult.Errors) //выводим ошибки
                ModelState.AddModelError(string.Empty, identityError.Description);

            return View(model);
        }

        public IActionResult AccessDenied() => View();

        [Authorize]
        public async Task<IActionResult> GetOrdersByUser([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrders(User.Identity.Name);
            var userOrder = orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Phone = o.Phone,
                Address = o.Address,
                TotalSum = o.Items.Sum(x => x.Price * x.Quantity)
            });

            return View(userOrder);
        }

    }
}
