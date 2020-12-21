using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrator)]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) => _productService = productService;
        

        public IActionResult Index() => View(_productService.GetProducts().Products.FromDTO());
        
        public IActionResult Details(int id)
        {
            var product =  _productService.GetProductById(id);
                
            if (product == null) return NotFound();
            
            return View(product.FromDTO());
        }

        //public IActionResult Create()
        //{
        //    ViewData["BrandId"] = new SelectList(_productService.Brands, "Id", "Id");
        //    ViewData["CategoryId"] = new SelectList(_productService.Categories, "Id", "Id");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Order,CategoryId,BrandId,ImageUrl,Price,Manufacturer,Id,Name")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _productService.Add(product);
        //        await _productService.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BrandId"] = new SelectList(_productService.Brands, "Id", "Id", product.BrandId);
        //    ViewData["CategoryId"] = new SelectList(_productService.Categories, "Id", "Id", product.CategoryId);
        //    return View(product);
        //}

        // Edit
        
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(product.FromDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(product.FromDTO());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Product product)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}
