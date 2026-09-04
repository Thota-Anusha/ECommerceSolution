using ECommerce.Models;

namespace ECommerce.Business.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();

        Task<Inventory?> GetInventoryByIdAsync(int id);

        Task<Inventory> CreateInventoryAsync(Inventory inventory);

        Task UpdateInventoryAsync(Inventory inventory);

        Task DeleteInventoryAsync(int id);
    }
}
