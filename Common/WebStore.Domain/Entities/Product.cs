using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Продукт</summary>
    [Table("Products")]
    public class Product : NamedEntity, IOrderedEntity
    {
        /// <summary>Очередность</summary>
        public int Order { get; set; }

        /// <summary>Идентификатор категории</summary>
        public int CategoryId { get; set; }

        /// <summary>Идентификатор бренда (при наличии)</summary>
        public int? BrandId { get; set; }

        /// <summary>Ссылка на изображение</summary>
        public string ImageUrl { get; set; }

        /// <summary>Цена</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>Изготовитель</summary>
        public string Manufacturer { get; set; }

        /// <summary>Категория</summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>Бренд</summary>
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
    }
}