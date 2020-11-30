using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities
{
    /// <summary>Заказ</summary>
    public class Order : NamedEntity
    {
        /// <summary>Номер телефона</summary>
        public string Phone { get; set; }

        /// <summary>Адрес</summary>
        public string Address { get; set; }
        
        /// <summary>Дата</summary>
        public DateTime Date { get; set; }

        /// <summary>Пользователь</summary>
        [Required]
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
