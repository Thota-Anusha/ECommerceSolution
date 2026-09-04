using ECommerce.Models;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        Task AddOrderAsync(Order order);

        Task AddOrderItemAsync(OrderItem orderItem);

        Task UpdateOrderAsync(Order order);

        Task SaveChangesAsync();
    }
}