using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BrandsViewComponent(IProductService productService) => _productService = productService;

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands()
        {
            var dbBrands = _productService.GetBrands();
            return dbBrands
                .Select(b => b.FromDTO())
                .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order,
            }).OrderBy(b => b.Order);
        }
    }

}
