using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;

namespace ECommerce.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task AddBulkProductsAsync(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var category = await _categoryRepository
                    .GetByIdAsync(product.CategoryId);

                if (category == null)
                {
                    throw new InvalidDataException(
                        $"CategoryId {product.CategoryId} does not exist.");
                }
            }

            await _productRepository.AddRangeAsync(products);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var category = await _categoryRepository
                .GetByIdAsync(product.CategoryId);

            if (category == null)
            {
                throw new InvalidDataException(
                    $"CategoryId {product.CategoryId} does not exist.");
            }

            return await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var category = await _categoryRepository
                .GetByIdAsync(product.CategoryId);

            if (category == null)
            {
                throw new InvalidDataException(
                    $"CategoryId {product.CategoryId} does not exist.");
            }

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
