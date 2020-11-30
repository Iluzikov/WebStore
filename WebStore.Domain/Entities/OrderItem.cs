using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [Required]
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
