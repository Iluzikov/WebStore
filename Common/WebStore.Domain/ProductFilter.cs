using System.Collections.Generic;

namespace WebStore.Domain
{
    /// <summary>
    /// Класс для фильтрации товаров
    /// </summary>
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int[] Ids { get; set; }
        public int Page { get; set; }
        public int? PageSize { get; set; }
    }
}
