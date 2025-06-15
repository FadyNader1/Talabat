using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Order;
using Talabat.Core.Interfaces;
using Talabat.DTO.OrderDTO;
using Talabat.Errors;

namespace Talabat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ordercontroller : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public Ordercontroller(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "Invalid Modelstate" }
                });
            var email = User.FindFirstValue(ClaimTypes.Email);
            if(email is null)
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "pleace register " }
                });
            var shippingaddress = mapper.Map<UserAddress, Address>(orderDTO.ShippingAddress);
            if (shippingaddress == null)
            {
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "Shipping address is required" }
                });
            }
            var result = await orderService.CreateOrderAsync(orderDTO.BasketId, email, orderDTO.DeliveryMethodId, shippingaddress);
            if (result == null)
                return BadRequest(new ApiValidationError()
                {
                   Errors= new List<string>() { "Faild in create order"}
                });
                    
            return Ok(result);

        }
        [HttpGet("CurrentUserOrders")]
        public async Task<ActionResult<IReadOnlyList<Order>>> CurrentUserOrders()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var result = await orderService.GetCurrentUserOrders(email);
            if (result is null)
                return NotFound(new ApiHandleError(404));
            return Ok(result);
        }

        [HttpGet("GetOrderById")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order is null)
                return NotFound(new ApiHandleError(404));
            return Ok(order);
        }
        [HttpDelete("CancelOrderById")]
        public async Task<ActionResult<string>> CancelOrderById(int id)
        {
            var order = await orderService.CancelOrderByIdAsync(id);
            return Ok(order);
        }
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAllOrders()
        {
            var orders = await orderService.GetAllOrders();
            if (orders is null)
                return NotFound(new ApiValidationError()
                {
                    Errors = new List<string>() { "Not found any order" }
                });
            return Ok(orders);

        }
    }
}
