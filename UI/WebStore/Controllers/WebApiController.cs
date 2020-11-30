using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestApi;

namespace WebStore.Controllers
{
    public class WebApiController : Controller
    {
        private readonly IValuesService _valueService;
        public WebApiController(IValuesService valuesService) => _valueService = valuesService;


        public IActionResult Index()
        {
            var values = _valueService.Get();
            return View(values);
        }

    }
}
