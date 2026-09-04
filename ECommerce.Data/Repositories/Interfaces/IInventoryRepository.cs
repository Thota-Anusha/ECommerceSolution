using ECommerce.Models;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();

        Task<Inventory?> GetByIdAsync(int id);

        Task<Inventory?> GetByProductIdAsync(int productId);

        Task<Inventory> AddAsync(Inventory inventory);

        Task UpdateAsync(Inventory inventory);

        Task DeleteAsync(int id);
    }
}
