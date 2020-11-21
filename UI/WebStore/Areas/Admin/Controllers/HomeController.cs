﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebStoreRole.Admins)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}