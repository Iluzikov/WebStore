using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.ViewComponents
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BreadCrumbsViewComponent(IProductService productService) => _productService = productService;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsViewModel();

            if(int.TryParse(Request.Query["CategoryId"], out var category_id))
            {
                model.Category = _productService.GetCategoryById(category_id).FromDTO();
                if (model.Category.ParentId != null)
                    model.Category.ParentCategory = _productService.GetCategoryById((int)model.Category.ParentId).FromDTO();
            }

            if (int.TryParse(Request.Query["BrandId"], out var brand_id))
            {
                model.Brand = _productService.GetBrandById(brand_id).FromDTO();
            }

            if(int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var product_id))
            {
                var product = _productService.GetProductById(product_id);
                if (product is not null)
                    model.Product = product.Name;
            }

            return View(model);
        }
    }
}
