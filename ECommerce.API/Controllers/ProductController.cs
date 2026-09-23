using ECommerce.Business.Interfaces;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Business.Excel;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ProductExcelService _productExcelService;

        public ProductController(
        IProductService productService,
        ProductExcelService productExcelService)
        {
            _productService = productService;
            _productExcelService = productExcelService;
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
            try
            {
                var createdProduct =
                    await _productService.CreateProductAsync(product);

                return Ok(createdProduct);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                await _productService.UpdateProductAsync(product);

                return NoContent();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
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
        // ==========================================
        // BULK UPLOAD - Admin only
        // ==========================================


        [HttpPost("bulk-upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload an Excel file.");
            }

            if (!Path.GetExtension(file.FileName)
                .Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only .xlsx Excel files are allowed.");
            }

            try
            {
                using var stream = file.OpenReadStream();

                var products = _productExcelService.ReadProducts(stream);

                if (products.Count == 0)
                {
                    return BadRequest(
                        "The Excel file does not contain any products.");
                }

                await _productService.AddBulkProductsAsync(products);

                return Ok(new
                {
                    message = "Products uploaded successfully.",
                    count = products.Count
                });
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        
    }
    }
}