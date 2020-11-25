using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration confogiration) : base(confogiration, WebAPI.Orders) { }

        public async Task<OrderDTO> CreateOrder(CreateOrderModel orderModel, string userName)
        {
            var response = await PostAsync($"{_serviceAddress}/{userName}", orderModel);
            return await response.Content.ReadAsAsync<OrderDTO>();
        }

        public async Task<OrderDTO> GetOrderById(int id) => await GetAsync<OrderDTO>($"{_serviceAddress}/{id}");

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName) =>
            await GetAsync<IEnumerable<OrderDTO>>($"{_serviceAddress}/user/{userName}");
    }
}
