using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string userName);
        Task<Order> GetOrderById(int id);
        Task<Order> CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}
