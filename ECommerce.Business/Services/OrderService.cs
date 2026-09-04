using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;

namespace ECommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;

        // ==========================================
        // Constructor
        // ==========================================

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
        }

        // ==========================================
        // Get My Orders
        // ==========================================

        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            return await _orderRepository
                .GetOrdersByUserIdAsync(userId);
        }

        // ==========================================
        // Get Order By ID
        // ==========================================

        public async Task<Order?> GetOrderByIdAsync(
            int orderId,
            int userId)
        {
            var order =
                await _orderRepository
                    .GetOrderByIdAsync(orderId);

            if (order == null ||
                order.UserId != userId)
            {
                return null;
            }

            return order;
        }

        // ==========================================
        // Create Order
        // ==========================================

        public async Task<Order?> CreateOrderAsync(int userId)
        {
            var cart =
                await _cartRepository
                    .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return null;
            }

            var cartItems =
                await _cartRepository
                    .GetCartItemsAsync(cart.CartId);

            if (cartItems.Count == 0)
            {
                return null;
            }

            // Check inventory before creating order
            foreach (var cartItem in cartItems)
            {
                var inventory =
                    await _inventoryRepository
                        .GetByProductIdAsync(
                            cartItem.ProductId);

                if (inventory == null ||
                    inventory.Quantity < cartItem.Quantity)
                {
                    return null;
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                Status = "Pending"
            };

            await _orderRepository
                .AddOrderAsync(order);

            await _orderRepository
                .SaveChangesAsync();

            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                var product =
                    await _productRepository
                        .GetByIdAsync(
                            cartItem.ProductId);

                var inventory =
                    await _inventoryRepository
                        .GetByProductIdAsync(
                            cartItem.ProductId);

                if (product == null ||
                    inventory == null)
                {
                    continue;
                }

                decimal itemTotal =
                    product.Price * cartItem.Quantity;

                totalAmount += itemTotal;

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = product.Price
                };

                await _orderRepository
                    .AddOrderItemAsync(orderItem);

                // Reduce stock
                inventory.Quantity -= cartItem.Quantity;

                await _inventoryRepository
                    .UpdateAsync(inventory);
            }

            order.TotalAmount = totalAmount;

            await _orderRepository
                .SaveChangesAsync();

            return order;
        }
    }
}