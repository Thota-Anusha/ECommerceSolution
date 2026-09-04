using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // ==========================================
        // GET CART - Logged-in user
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();

            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return Ok(cart);
        }

        // ==========================================
        // ADD PRODUCT TO CART
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> AddToCart(
            int productId,
            int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            var userId = GetUserId();

            var cart = await _cartService.AddToCartAsync(
                userId,
                productId,
                quantity);

            return Ok(cart);
        }

        // ==========================================
        // UPDATE CART ITEM
        // ==========================================

        [HttpPut]
        public async Task<IActionResult> UpdateCartItem(
            int productId,
            int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            var userId = GetUserId();

            var result = await _cartService.UpdateCartItemAsync(
                userId,
                productId,
                quantity);

            if (!result)
            {
                return NotFound("Cart item not found.");
            }

            return NoContent();
        }

        // ==========================================
        // REMOVE PRODUCT FROM CART
        // ==========================================

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(
            int productId)
        {
            var userId = GetUserId();

            var result = await _cartService.RemoveFromCartAsync(
                userId,
                productId);

            if (!result)
            {
                return NotFound("Cart item not found.");
            }

            return NoContent();
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