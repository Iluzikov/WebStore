namespace WebStore.Domain.DTO.Products
{
    public class ProductDTO
    {
        /// <summary>Идентификатор продукта</summary>
        public int Id { get; set; }

        /// <summary>Наименование продукта</summary>
        public string Name { get; set; }

        /// <summary>Очередность</summary>
        public int Order { get; set; }

        /// <summary>Цена</summary>
        public decimal Price { get; set; }

        /// <summary>Ссылка на изображение</summary>
        public string ImageUrl { get; set; }

        /// <summary>Бренд</summary>
        public BrandDTO Brand { get; set; }

        /// <summary>Категория</summary>
        public CategoryDTO Category { get; set; }
    }
}
