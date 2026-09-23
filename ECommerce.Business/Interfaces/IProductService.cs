using ECommerce.Models;

namespace ECommerce.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task AddBulkProductsAsync(IEnumerable<Product> products);

        Task<Product?> GetProductByIdAsync(int id);

        Task<Product> CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(int id);
    }
}
