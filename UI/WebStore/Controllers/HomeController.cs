using Microsoft.AspNetCore.Mvc;
using System;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult Cart() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Login() => View();

        public IActionResult PageNotFound() => View();

        public IActionResult Throw(string id) =>
            throw new ApplicationException($"Исключение: {id ?? "<null>"}");

        public IActionResult ErrorStatus(string Code) => Code switch
        {
            "404" => RedirectToAction(nameof(PageNotFound)),
            _ => Content($"Error {Code}")
        };

    }
}
