using System.Collections.Generic;

namespace WebStore.Domain.ViewModels
{
    public class SelectableCategoriesViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public int? CurrentCategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
