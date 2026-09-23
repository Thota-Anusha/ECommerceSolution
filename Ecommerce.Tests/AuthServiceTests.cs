using ECommerce.Business.Services;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Ecommerce.Tests
{
    public class AuthServiceTests
    {
        private IConfiguration GetConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "ThisIsMySuperSecretJwtKey1234567890" },
                { "Jwt:Issuer", "ECommerceAPI" },
                { "Jwt:Audience", "ECommerceClient" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }


        [Fact]
        public async Task RegisterAsync_CreatesUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync("test@gmail.com"))
                .ReturnsAsync((User?)null);

            userRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            var configuration = GetConfiguration();

            var authService = new AuthService(
                userRepositoryMock.Object,
                configuration);

            // Act
            var result = await authService.RegisterAsync(
                "Anusha",
                "test@gmail.com",
                "Password123");

            // Assert
            Assert.True(result);

            userRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<User>(u =>
                    u.Name == "Anusha" &&
                    u.Email == "test@gmail.com" &&
                    u.Role == "Customer")),
                Times.Once);
        }


        [Fact]
        public async Task RegisterAsync_ReturnsFalse_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = 1,
                Name = "Existing User",
                Email = "test@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = "Customer"
            };

            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync("test@gmail.com"))
                .ReturnsAsync(existingUser);

            var configuration = GetConfiguration();

            var authService = new AuthService(
                userRepositoryMock.Object,
                configuration);

            // Act
            var result = await authService.RegisterAsync(
                "Anusha",
                "test@gmail.com",
                "Password123");

            // Assert
            Assert.False(result);

            userRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<User>()),
                Times.Never);
        }


        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync("unknown@gmail.com"))
                .ReturnsAsync((User?)null);

            var configuration = GetConfiguration();

            var authService = new AuthService(
                userRepositoryMock.Object,
                configuration);

            // Act
            var result = await authService.LoginAsync(
                "unknown@gmail.com",
                "Password123");

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsWrong()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Name = "Anusha",
                Email = "test@gmail.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                Role = "Customer"
            };

            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            var configuration = GetConfiguration();

            var authService = new AuthService(
                userRepositoryMock.Object,
                configuration);

            // Act
            var result = await authService.LoginAsync(
                "test@gmail.com",
                "WrongPassword");

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Name = "Anusha",
                Email = "test@gmail.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = "Customer"
            };

            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            var configuration = GetConfiguration();

            var authService = new AuthService(
                userRepositoryMock.Object,
                configuration);

            // Act
            var result = await authService.LoginAsync(
                "test@gmail.com",
                "Password123");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}