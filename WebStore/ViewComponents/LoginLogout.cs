using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewComponents
{
    public class LoginLogout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
