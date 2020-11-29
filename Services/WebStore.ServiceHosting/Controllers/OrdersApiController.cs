using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>API управления заказами</summary>
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;
        public OrdersApiController(IOrderService orderService) => _orderService = orderService;

        /// <summary>Создание заказа</summary>
        /// <param name="orderModel">Заказ</param>
        /// <param name="userName">Пользователь</param>
        /// <returns>Созданный заказ</returns>
        [HttpPost("{userName}")]
        public async Task<OrderDTO> CreateOrder([FromBody]CreateOrderModel orderModel, string userName) =>
            await _orderService.CreateOrder(orderModel, userName);

        /// <summary>Получение заказа по идентификатору</summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns>Заказ</returns>
        [HttpGet("{id}")]
        public async Task<OrderDTO> GetOrderById(int id) => await _orderService.GetOrderById(id);

        /// <summary>Получение заказов пользователя</summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список заказов пользователя</returns>
        [HttpGet("user/{userName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName) => await _orderService.GetUserOrders(userName);
    }
}
