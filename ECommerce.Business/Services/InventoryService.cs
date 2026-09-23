using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;

namespace ECommerce.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _inventoryRepository.GetByIdAsync(id);
        }

        public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
        {
            var product = await _productRepository.GetByIdAsync(inventory.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException(
                    $"Product with ID {inventory.ProductId} does not exist.");
            }

            var existingInventory = await _inventoryRepository
                .GetByProductIdAsync(inventory.ProductId);

            if (existingInventory != null)
            {
                throw new InvalidOperationException(
                    $"Inventory already exists for Product ID {inventory.ProductId}.");
            }

            return await _inventoryRepository.AddAsync(inventory);
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            await _inventoryRepository.UpdateAsync(inventory);
        }

        public async Task DeleteInventoryAsync(int id)
        {
            await _inventoryRepository.DeleteAsync(id);
        }
    }
}