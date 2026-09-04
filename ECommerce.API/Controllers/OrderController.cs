using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ==========================================
        // GET MY ORDERS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();

            var orders =
                await _orderService.GetMyOrdersAsync(userId);

            return Ok(orders);
        }

        // ==========================================
        // GET ORDER BY ID
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = GetUserId();

            var order =
                await _orderService.GetOrderByIdAsync(
                    id,
                    userId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // ==========================================
        // CREATE ORDER
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = GetUserId();

            var order =
                await _orderService.CreateOrderAsync(userId);

            if (order == null)
            {
                return BadRequest("Cart is empty or does not exist.");
            }

            return Ok(order);
        }

        // ==========================================
        // GET LOGGED-IN USER ID
        // ==========================================

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!
            );
        }
    }
}