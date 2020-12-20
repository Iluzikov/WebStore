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

        public IViewComponentResult Invoke(string BrandId) => 
            View(new SelectableBrandsViewModel 
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(BrandId, out var id) ? id : (int?)null
            });

        private IEnumerable<BrandViewModel> GetBrands()
        {
            var dbBrands = _productService.GetBrands();
            return dbBrands
                //.Select(b => b.FromDTO())
                .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order,
                ProductsCount = b.ProductsCount,
            }).OrderBy(b => b.Order);
        }
    }

}
