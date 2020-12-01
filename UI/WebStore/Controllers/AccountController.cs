using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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

            using (_logger.BeginScope("Вход пользователя {0} в систему", model.UserName))
            {
                var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

                if (loginResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь успешно вошел в систему");
                    if (Url.IsLocalUrl(model.ReturnUrl)) //если ReturnUrl - локальный
                    {
                        _logger.LogInformation("Перенаправляем вошедшего пользователя {0} на адрес {1}",
                            model.UserName, model.ReturnUrl);
                        return Redirect(model.ReturnUrl);  //перенаправляем туда, откуда пришли
                    }
                    _logger.LogInformation("Перенаправляем вошедшего пользователя {0} на главную страницу", model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                _logger.LogWarning("Ошибка ввода пароля при входе пользователя {0} в систему", model.UserName);
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль!");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity!.Name;
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Пользователь {0} вышел из системы", user_name);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using(_logger.BeginScope("Регистрация нового пользователя {0}", model.UserName))
            {
                _logger.LogInformation("Регистрация нового пользователя {0}", model.UserName);

                var user = new User { UserName = model.UserName/*, Email = model.Email */};
                var registrationResult = await _userManager.CreateAsync(user, model.Password);

                if (registrationResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} успешно зарегистрирован", model.UserName);
                    await _userManager.AddToRoleAsync(user, Role.User);
                    _logger.LogInformation("Пользователю {0} назначена роль {1}", model.UserName, Role.User);
                    await _signInManager.SignInAsync(user, false); //если успешно - логинимся
                    _logger.LogInformation("Пользователь {0} автоматически вощел в аккаунт в первый раз", model.UserName);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var identityError in registrationResult.Errors) //выводим ошибки
                    ModelState.AddModelError(string.Empty, identityError.Description);

                _logger.LogWarning("Ошибка при регистрации нового пользователя {0} {1}",
                    model.UserName,
                    string.Join(",", registrationResult.Errors.Select(e => e.Description)));
            }
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
