using ECommerce.Business.Interfaces;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ==========================================
        // GET - Any logged-in user
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products =
                await _productService.GetAllProductsAsync();

            return Ok(products);
        }

        // ==========================================
        // GET BY ID - Any logged-in user
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product =
                await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // ==========================================
        // POST - Admin only
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var createdProduct =
                await _productService.CreateProductAsync(product);

            return Ok(createdProduct);
        }

        // ==========================================
        // PUT - Admin only
        // ==========================================

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(product);

            return NoContent();
        }

        // ==========================================
        // DELETE - Admin only
        // ==========================================

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}