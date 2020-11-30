using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<(ProductViewModel product, int quantity)> Items { get; set; }
        public int ItemsCount => Items?.Sum(x => x.quantity) ?? 0;

        //вычисляем сумму всех товаров
        public decimal TotalPrice => Items?.Sum(x => x.product.Price * x.quantity) ?? 0m;
    }
}
