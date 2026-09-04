using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;

namespace ECommerce.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        // ==========================================
        // Get Payment By ID
        // ==========================================

        public async Task<Payment?> GetPaymentByIdAsync(
            int paymentId,
            int userId)
        {
            var payment =
                await _paymentRepository
                    .GetByIdAsync(paymentId);

            if (payment == null)
            {
                return null;
            }

            var order =
                await _orderRepository
                    .GetOrderByIdAsync(payment.OrderId);

            if (order == null || order.UserId != userId)
            {
                return null;
            }

            return payment;
        }

        // ==========================================
        // Get Payment By Order ID
        // ==========================================

        public async Task<Payment?> GetPaymentByOrderIdAsync(
            int orderId,
            int userId)
        {
            var order =
                await _orderRepository
                    .GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != userId)
            {
                return null;
            }

            return await _paymentRepository
                .GetByOrderIdAsync(orderId);
        }

        // ==========================================
        // Create Payment
        // ==========================================

        public async Task<Payment?> CreatePaymentAsync(
            int orderId,
            int userId)
        {
            var order =
                await _orderRepository
                    .GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != userId)
            {
                return null;
            }

            // Prevent duplicate payment
            var existingPayment =
                await _paymentRepository
                    .GetByOrderIdAsync(orderId);

            if (existingPayment != null)
            {
                return existingPayment;
            }

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = order.TotalAmount,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Paid"
            };
            order.Status = "Paid";

            await _orderRepository.UpdateOrderAsync(order);

            await _paymentRepository.AddAsync(payment);

            await _paymentRepository.SaveChangesAsync();

            return payment;
        }
    }
}