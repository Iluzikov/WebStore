using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        /// <summary>Цена</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>Количество</summary>
        public int Quantity { get; set; }

        /// <summary>Заказ</summary>
        [Required]
        public virtual Order Order { get; set; }

        /// <summary>Продукт</summary>
        public virtual Product Product { get; set; }
    }
}
