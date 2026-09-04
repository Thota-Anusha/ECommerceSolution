using ECommerce.Models;

namespace ECommerce.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentByIdAsync(
            int paymentId,
            int userId);

        Task<Payment?> GetPaymentByOrderIdAsync(
            int orderId,
            int userId);

        Task<Payment?> CreatePaymentAsync(
            int orderId,
            int userId);
    }
}