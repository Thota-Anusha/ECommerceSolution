using ECommerce.Business.Interfaces;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(
            IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // ==========================================
        // GET ALL - Any logged-in user
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventory =
                await _inventoryService.GetAllInventoryAsync();

            return Ok(inventory);
        }

        // ==========================================
        // GET BY ID - Any logged-in user
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // ==========================================
        // POST - Admin only
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateInventory(
            Inventory inventory)
        {
            try
            {
                var result =
                    await _inventoryService.CreateInventoryAsync(
                        inventory);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // PUT - Admin only
        // ==========================================

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInventory(
            int id,
            Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return BadRequest();
            }

            await _inventoryService.UpdateInventoryAsync(
                inventory);

            return NoContent();
        }

        // ==========================================
        // DELETE - Admin only
        // ==========================================

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);

            return NoContent();
        }
    }
}