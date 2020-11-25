using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService) => _orderService = orderService;

        [HttpPost("{userName}")]
        public async Task<OrderDTO> CreateOrder([FromBody]CreateOrderModel orderModel, string userName) =>
            await _orderService.CreateOrder(orderModel, userName);

        [HttpGet("{id}")]
        public async Task<OrderDTO> GetOrderById(int id) => await _orderService.GetOrderById(id);

        [HttpGet("user/{userName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName) => await _orderService.GetUserOrders(userName);
    }
}
