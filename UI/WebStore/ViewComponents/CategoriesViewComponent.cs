using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public CategoriesViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(string CategoryId)
        {
            var category_id = int.TryParse(CategoryId, out var id) ? id : (int?)null;
            var categories = GetCategories(category_id, out var parent_category_id);

            return View(new SelectableCategoriesViewModel 
            {
                Categories = categories,
                CurrentCategoryId = category_id,
                ParentCategoryId = parent_category_id
            });
        }

        private IEnumerable<CategoryViewModel> GetCategories(int? categoryId, out int? parentCategoryId)
        {
            parentCategoryId = null;

            var categories = _productService.GetCategories();

            // получим и заполним родительские категории
            var parentSections = categories.Where(p => !p.ParentId.HasValue).ToArray();
            var parentCategories = new List<CategoryViewModel>();
            
            foreach (var parentCategory in parentSections)
            {
                parentCategories.Add(new CategoryViewModel()
                {
                    Id = parentCategory.Id,
                    Name = parentCategory.Name,
                    Order = parentCategory.Order,
                    ParentCategory = null
                });
            }

            // получим и заполним дочерние категории
            foreach (var CategoryViewModel in parentCategories)
            {
                var childCategories = categories.Where(c => c.ParentId == CategoryViewModel.Id);
                foreach (var childCategory in childCategories)
                {
                    if (childCategory.Id == categoryId)
                        parentCategoryId = childCategory.ParentId;

                    CategoryViewModel.ChildCategories.Add(new CategoryViewModel()
                    {
                        Id = childCategory.Id,
                        Name = childCategory.Name,
                        Order = childCategory.Order,
                        ParentCategory = CategoryViewModel
                    });
                }
                CategoryViewModel.ChildCategories = CategoryViewModel.ChildCategories.OrderBy(c => c.Order).ToList();
            }
            parentCategories = parentCategories.OrderBy(c => c.Order).ToList();
            return parentCategories;
        }

    }
}