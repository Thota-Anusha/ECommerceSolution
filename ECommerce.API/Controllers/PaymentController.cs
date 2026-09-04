using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // ==========================================
        // GET PAYMENT BY ID
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var userId = GetUserId();

            var payment =
                await _paymentService.GetPaymentByIdAsync(
                    id,
                    userId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            return Ok(payment);
        }

        // ==========================================
        // GET PAYMENT BY ORDER ID
        // ==========================================

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(
            int orderId)
        {
            var userId = GetUserId();

            var payment =
                await _paymentService.GetPaymentByOrderIdAsync(
                    orderId,
                    userId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            return Ok(payment);
        }

        // ==========================================
        // CREATE PAYMENT
        // ==========================================

        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreatePayment(
            int orderId)
        {
            var userId = GetUserId();

            var payment =
                await _paymentService.CreatePaymentAsync(
                    orderId,
                    userId);

            if (payment == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(payment);
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