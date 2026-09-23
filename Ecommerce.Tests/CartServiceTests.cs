using ECommerce.Business.Services;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Moq;

namespace Ecommerce.Tests
{
    public class CartServiceTests
    {
        [Fact]
        public async Task GetCartAsync_ReturnsCart_WhenCartExists()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.GetCartAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CartId);
            Assert.Equal(10, result.UserId);
        }


        [Fact]
        public async Task AddToCartAsync_CreatesCartAndAddsItem_WhenCartDoesNotExist()
        {
            // Arrange
            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync((Cart?)null);

            cartRepositoryMock
                .Setup(repo => repo.AddCartAsync(It.IsAny<Cart>()))
                .Callback<Cart>(cart =>
                {
                    cart.CartId = 1;
                });

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync((CartItem?)null);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.AddToCartAsync(
                10,
                5,
                2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.UserId);

            cartRepositoryMock.Verify(
                repo => repo.AddCartAsync(It.Is<Cart>(
                    c => c.UserId == 10)),
                Times.Once);

            cartRepositoryMock.Verify(
                repo => repo.AddCartItemAsync(It.Is<CartItem>(
                    item =>
                        item.CartId == 1 &&
                        item.ProductId == 5 &&
                        item.Quantity == 2)),
                Times.Once);

            cartRepositoryMock.Verify(
                repo => repo.SaveChangesAsync(),
                Times.AtLeastOnce);
        }


        [Fact]
        public async Task AddToCartAsync_IncreasesQuantity_WhenItemAlreadyExists()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var existingItem = new CartItem
            {
                CartItemId = 1,
                CartId = 1,
                ProductId = 5,
                Quantity = 2
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync(existingItem);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.AddToCartAsync(
                10,
                5,
                3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, existingItem.Quantity);

            cartRepositoryMock.Verify(
                repo => repo.UpdateCartItemAsync(existingItem),
                Times.Once);

            cartRepositoryMock.Verify(
                repo => repo.AddCartItemAsync(It.IsAny<CartItem>()),
                Times.Never);
        }


        [Fact]
        public async Task UpdateCartItemAsync_ReturnsFalse_WhenCartDoesNotExist()
        {
            // Arrange
            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync((Cart?)null);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.UpdateCartItemAsync(
                10,
                5,
                4);

            // Assert
            Assert.False(result);

            cartRepositoryMock.Verify(
                repo => repo.UpdateCartItemAsync(It.IsAny<CartItem>()),
                Times.Never);
        }


        [Fact]
        public async Task UpdateCartItemAsync_ReturnsFalse_WhenItemDoesNotExist()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync((CartItem?)null);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.UpdateCartItemAsync(
                10,
                5,
                4);

            // Assert
            Assert.False(result);

            cartRepositoryMock.Verify(
                repo => repo.UpdateCartItemAsync(It.IsAny<CartItem>()),
                Times.Never);
        }


        [Fact]
        public async Task UpdateCartItemAsync_UpdatesQuantity_WhenItemExists()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var item = new CartItem
            {
                CartItemId = 1,
                CartId = 1,
                ProductId = 5,
                Quantity = 2
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync(item);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.UpdateCartItemAsync(
                10,
                5,
                7);

            // Assert
            Assert.True(result);
            Assert.Equal(7, item.Quantity);

            cartRepositoryMock.Verify(
                repo => repo.UpdateCartItemAsync(item),
                Times.Once);

            cartRepositoryMock.Verify(
                repo => repo.SaveChangesAsync(),
                Times.Once);
        }


        [Fact]
        public async Task RemoveFromCartAsync_ReturnsFalse_WhenCartDoesNotExist()
        {
            // Arrange
            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync((Cart?)null);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.RemoveFromCartAsync(
                10,
                5);

            // Assert
            Assert.False(result);

            cartRepositoryMock.Verify(
                repo => repo.DeleteCartItemAsync(It.IsAny<CartItem>()),
                Times.Never);
        }


        [Fact]
        public async Task RemoveFromCartAsync_ReturnsFalse_WhenItemDoesNotExist()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync((CartItem?)null);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.RemoveFromCartAsync(
                10,
                5);

            // Assert
            Assert.False(result);

            cartRepositoryMock.Verify(
                repo => repo.DeleteCartItemAsync(It.IsAny<CartItem>()),
                Times.Never);
        }


        [Fact]
        public async Task RemoveFromCartAsync_RemovesItem_WhenItemExists()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 10
            };

            var item = new CartItem
            {
                CartItemId = 1,
                CartId = 1,
                ProductId = 5,
                Quantity = 2
            };

            var cartRepositoryMock = new Mock<ICartRepository>();

            cartRepositoryMock
                .Setup(repo => repo.GetCartByUserIdAsync(10))
                .ReturnsAsync(cart);

            cartRepositoryMock
                .Setup(repo => repo.GetCartItemAsync(1, 5))
                .ReturnsAsync(item);

            var cartService = new CartService(
                cartRepositoryMock.Object);

            // Act
            var result = await cartService.RemoveFromCartAsync(
                10,
                5);

            // Assert
            Assert.True(result);

            cartRepositoryMock.Verify(
                repo => repo.DeleteCartItemAsync(item),
                Times.Once);

            cartRepositoryMock.Verify(
                repo => repo.SaveChangesAsync(),
                Times.Once);
        }
    }
}