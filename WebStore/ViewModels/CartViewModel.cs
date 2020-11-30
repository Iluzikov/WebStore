using System.Collections.Generic;
using System.Linq;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; }
        public int ItemsCount => Items?.Sum(x => x.Value) ?? 0;

        //вычисляем сумму всех товаров
        public decimal ProductsPriceSum => Items?.Sum(x => x.Key.Price * x.Value) ?? 0;
    }
}
