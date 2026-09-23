using ECommerce.Business.Services;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Moq;

namespace Ecommerce.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Laptop",
                Description = "Dell Laptop",
                Price = 55000,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                });

            productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            var result = await productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("Laptop", result.Name);
            Assert.Equal(55000, result.Price);
        }
        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            Name = "Laptop",
            Description = "Dell Laptop",
            Price = 55000,
            CategoryId = 1
        },
        new Product
        {
            ProductId = 2,
            Name = "Mouse",
            Description = "Wireless Mouse",
            Price = 1200,
            CategoryId = 2
        }
    };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                });

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Category
                {
                    CategoryId = 2,
                    Name = "Accessories"
                });

            productRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            var result = await productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task CreateProductAsync_ReturnsCreatedProduct()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 3,
                Name = "Keyboard",
                Description = "Mechanical Keyboard",
                Price = 2500,
                CategoryId = 2
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Category
                {
                    CategoryId = 2,
                    Name = "Accessories"
                });

            productRepositoryMock
                .Setup(repo => repo.AddAsync(product))
                .ReturnsAsync(product);

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            var result = await productService.CreateProductAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.ProductId);
            Assert.Equal("Keyboard", result.Name);
            Assert.Equal(2500, result.Price);

            productRepositoryMock.Verify(
                repo => repo.AddAsync(product),
                Times.Once);
        }
        [Fact]
        public async Task UpdateProductAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Updated Laptop",
                Description = "Updated Dell Laptop",
                Price = 60000,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                });

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            await productService.UpdateProductAsync(product);

            // Assert
            productRepositoryMock.Verify(
                repo => repo.UpdateAsync(product),
                Times.Once);
        }
        [Fact]
        public async Task DeleteProductAsync_CallsRepositoryDelete()
        {
            // Arrange
            int productId = 1;

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            await productService.DeleteProductAsync(productId);

            // Assert
            productRepositoryMock.Verify(
                repo => repo.DeleteAsync(productId),
                Times.Once);
        }
        [Fact]
        public async Task AddBulkProductsAsync_AddsProducts_WhenCategoriesExist()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            Name = "Laptop",
            Description = "Dell Laptop",
            Price = 55000,
            CategoryId = 1
        },
        new Product
        {
            ProductId = 2,
            Name = "Mouse",
            Description = "Wireless Mouse",
            Price = 1200,
            CategoryId = 2
        }
    };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                });

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Category
                {
                    CategoryId = 2,
                    Name = "Accessories"
                });

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act
            await productService.AddBulkProductsAsync(products);

            // Assert
            productRepositoryMock.Verify(
                repo => repo.AddRangeAsync(products),
                Times.Once);
        }
        [Fact]
        public async Task AddBulkProductsAsync_ThrowsException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            Name = "Laptop",
            Description = "Dell Laptop",
            Price = 55000,
            CategoryId = 999
        }
    };

            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Category?)null);

            var productService = new ProductService(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(
                () => productService.AddBulkProductsAsync(products));

            // Verify that products were NOT inserted
            productRepositoryMock.Verify(
                repo => repo.AddRangeAsync(It.IsAny<IEnumerable<Product>>()),
                Times.Never);
        }
    }
}