using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    [Table("Brands")]
    public class Brand : NamedEntity, IOrderedEntity
    {
        /// <summary>Очередность</summary>
        public int Order { get; set; }

        /// <summary>Список Продуктов</summary>
        public virtual ICollection<Product> Products { get; set; }

    }
}