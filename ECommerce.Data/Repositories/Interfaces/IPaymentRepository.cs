using ECommerce.Models;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int paymentId);

        Task<Payment?> GetByOrderIdAsync(int orderId);

        Task AddAsync(Payment payment);

        Task SaveChangesAsync();
    }
}