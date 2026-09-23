using ECommerce.Business.Services;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Moq;

namespace ECommerce.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;

        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _productRepositoryMock.Object,
                _inventoryRepositoryMock.Object
            );
        }

        // 1. GetMyOrdersAsync
        [Fact]
        public async Task GetMyOrdersAsync_ReturnsUserOrders()
        {
            // Arrange
            int userId = 1;

            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    UserId = userId,
                    TotalAmount = 500,
                    Status = "Pending"
                },
                new Order
                {
                    OrderId = 2,
                    UserId = userId,
                    TotalAmount = 300,
                    Status = "Completed"
                }
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrdersByUserIdAsync(userId))
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetMyOrdersAsync(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(orders, result);
        }

        // 2. GetOrderByIdAsync - correct user
        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder_WhenOrderBelongsToUser()
        {
            // Arrange
            int orderId = 1;
            int userId = 10;

            var order = new Order
            {
                OrderId = orderId,
                UserId = userId,
                TotalAmount = 1000,
                Status = "Pending"
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(userId, result.UserId);
        }

        // 3. GetOrderByIdAsync - different user
        [Fact]
        public async Task GetOrderByIdAsync_ReturnsNull_WhenOrderBelongsToAnotherUser()
        {
            // Arrange
            int orderId = 1;
            int actualUserId = 10;
            int requestingUserId = 20;

            var order = new Order
            {
                OrderId = orderId,
                UserId = actualUserId,
                TotalAmount = 1000,
                Status = "Pending"
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(
                orderId,
                requestingUserId);

            // Assert
            Assert.Null(result);
        }

        // 4. GetOrderByIdAsync - order doesn't exist
        [Fact]
        public async Task GetOrderByIdAsync_ReturnsNull_WhenOrderDoesNotExist()
        {
            // Arrange
            int orderId = 999;
            int userId = 1;

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(
                orderId,
                userId);

            // Assert
            Assert.Null(result);
        }

        // 5. CreateOrderAsync - cart doesn't exist
        [Fact]
        public async Task CreateOrderAsync_ReturnsNull_WhenCartDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _cartRepositoryMock
                .Setup(r => r.GetCartByUserIdAsync(userId))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await _orderService.CreateOrderAsync(userId);

            // Assert
            Assert.Null(result);

            _orderRepositoryMock.Verify(
                r => r.AddOrderAsync(It.IsAny<Order>()),
                Times.Never);
        }

        // 6. CreateOrderAsync - empty cart
        [Fact]
        public async Task CreateOrderAsync_ReturnsNull_WhenCartIsEmpty()
        {
            // Arrange
            int userId = 1;

            var cart = new Cart
            {
                CartId = 1,
                UserId = userId
            };

            _cartRepositoryMock
                .Setup(r => r.GetCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            _cartRepositoryMock
                .Setup(r => r.GetCartItemsAsync(cart.CartId))
                .ReturnsAsync(new List<CartItem>());

            // Act
            var result = await _orderService.CreateOrderAsync(userId);

            // Assert
            Assert.Null(result);

            _orderRepositoryMock.Verify(
                r => r.AddOrderAsync(It.IsAny<Order>()),
                Times.Never);
        }

        // 7. CreateOrderAsync - insufficient inventory
        [Fact]
        public async Task CreateOrderAsync_ThrowsException_WhenInventoryIsInsufficient()
        {
            // Arrange
            int userId = 1;

            var cart = new Cart
            {
                CartId = 1,
                UserId = userId
            };

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartItemId = 1,
                    CartId = cart.CartId,
                    ProductId = 1,
                    Quantity = 5
                }
            };

            var inventory = new Inventory
            {
                ProductId = 1,
                Quantity = 2
            };

            _cartRepositoryMock
                .Setup(r => r.GetCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            _cartRepositoryMock
                .Setup(r => r.GetCartItemsAsync(cart.CartId))
                .ReturnsAsync(cartItems);

            _inventoryRepositoryMock
                .Setup(r => r.GetByProductIdAsync(1))
                .ReturnsAsync(inventory);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _orderService.CreateOrderAsync(userId));

            Assert.Equal(
                "Insufficient inventory for Product ID 1. Available quantity: 2.",
                ex.Message);

            _orderRepositoryMock.Verify(
                r => r.AddOrderAsync(It.IsAny<Order>()),
                Times.Never);
        }

        // 8. CreateOrderAsync - successful order
        [Fact]
        public async Task CreateOrderAsync_CreatesOrderSuccessfully()
        {
            // Arrange
            int userId = 1;

            var cart = new Cart
            {
                CartId = 1,
                UserId = userId
            };

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartItemId = 1,
                    CartId = cart.CartId,
                    ProductId = 1,
                    Quantity = 2
                }
            };

            var product = new Product
            {
                ProductId = 1,
                Price = 250
            };

            var inventory = new Inventory
            {
                ProductId = 1,
                Quantity = 10
            };

            _cartRepositoryMock
                .Setup(r => r.GetCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            _cartRepositoryMock
                .Setup(r => r.GetCartItemsAsync(cart.CartId))
                .ReturnsAsync(cartItems);

            _inventoryRepositoryMock
                .Setup(r => r.GetByProductIdAsync(1))
                .ReturnsAsync(inventory);

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            _orderRepositoryMock
                .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order =>
                {
                    order.OrderId = 100;
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateOrderAsync(userId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(userId, result.UserId);
            Assert.Equal(500, result.TotalAmount);
            Assert.Equal("Pending", result.Status);

            _orderRepositoryMock.Verify(
                r => r.AddOrderAsync(It.IsAny<Order>()),
                Times.Once);

            _orderRepositoryMock.Verify(
                r => r.AddOrderItemAsync(
                    It.Is<OrderItem>(item =>
                        item.OrderId == 100 &&
                        item.ProductId == 1 &&
                        item.Quantity == 2 &&
                        item.Price == 250)),
                Times.Once);

            _inventoryRepositoryMock.Verify(
                r => r.UpdateAsync(
                    It.Is<Inventory>(i =>
                        i.ProductId == 1 &&
                        i.Quantity == 8)),
                Times.Once);

            _orderRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Exactly(2));
        }
    }
}