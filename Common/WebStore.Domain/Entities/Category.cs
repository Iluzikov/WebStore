using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Категория</summary>
    [Table("Categories")]
    public class Category : NamedEntity, IOrderedEntity
    {
        /// <summary>Идентификатор родительской секция (при наличии)</summary>
        public int? ParentId { get; set; }

        /// <summary>Очередность</summary>
        public int Order { get; set; }

        /// <summary>Родительская категория</summary>
        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }

        /// <summary>продуктов</summary>
        public virtual ICollection<Product> Products { get; set; }

    }
}