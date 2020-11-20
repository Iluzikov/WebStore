using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services
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

        public async Task<Order> CreateOrder(OrderViewModel orderModel, CartViewModel cart, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user is null) throw new InvalidOperationException($"Пользователь {userName} на найден");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            
            var order = new Order()
            {
                Name = orderModel.Name,
                Address = orderModel.Address,
                Phone = orderModel.Phone,
                Date = DateTime.Now,
                User = user
            };

            foreach (var (product_model, quantity) in cart.Items)
            {
                var product = await _context.Products.FindAsync(product_model.Id);
                if (product is null) continue;

                var order_Item = new OrderItem()
                {
                    Order = order,
                    Price = product.Price,
                    Quantity = quantity,
                    Product = product
                };
                order.Items.Add(order_Item);
            }
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(order => order.User)
                .FirstOrDefaultAsync(o => o.Id.Equals(id));
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var res = await _context.Orders
                .Include(order => order.User)
                .Include(order => order.Items)
                .Where(o => o.User.UserName.Equals(userName))
                .ToArrayAsync();
            return res;
        }
    }
}
