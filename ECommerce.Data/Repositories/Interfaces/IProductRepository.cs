using ECommerce.Models;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task AddRangeAsync(IEnumerable<Product> products);

        Task<Product?> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}
