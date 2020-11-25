using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Products.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;

        public SqlOrderService(WebStoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<OrderDTO> CreateOrder(CreateOrderModel orderModel, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) throw new InvalidOperationException($"Пользователь {userName} на найден");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var order = new Order()
            {
                Name = orderModel.Order.Name,
                Address = orderModel.Order.Address,
                Phone = orderModel.Order.Phone,
                Date = DateTime.Now,
                User = user
            };

            foreach (var item in orderModel.Items)
            {
                var product = await _context.Products.FindAsync(item.Id);
                if (product is null) continue;

                var order_Item = new OrderItem()
                {
                    Order = order,
                    Price = product.Price,
                    Quantity = item.Id,
                    Product = product
                };
                order.Items.Add(order_Item);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order.ToDTO();
        }

        public async Task<OrderDTO> GetOrderById(int id) => (await _context.Orders
                .Include(order => order.User)
                .FirstOrDefaultAsync(o => o.Id.Equals(id))).ToDTO();

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName) => (await _context.Orders
                .Include(order => order.User)
                .Include(order => order.Items)
                .Where(o => o.User.UserName.Equals(userName))
                .ToArrayAsync()).Select(o => o.ToDTO());

    }
}
