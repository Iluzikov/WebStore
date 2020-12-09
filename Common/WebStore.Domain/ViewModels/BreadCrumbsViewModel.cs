using WebStore.Domain.Entities;

namespace WebStore.Domain.ViewModels
{
    public class BreadCrumbsViewModel
    {
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public string Product { get; set; }
    }
}
