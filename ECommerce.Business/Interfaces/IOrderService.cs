using ECommerce.Models;

namespace ECommerce.Business.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> GetOrderByIdAsync(
            int orderId,
            int userId);

        Task<List<Order>> GetMyOrdersAsync(
            int userId);

        Task<Order?> CreateOrderAsync(
            int userId);
    }
}