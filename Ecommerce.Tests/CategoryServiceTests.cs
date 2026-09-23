using ECommerce.Business.Services;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Moq;

namespace Ecommerce.Tests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Accessories"
                }
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            var categoryService = new CategoryService(
                categoryRepositoryMock.Object);

            // Act
            var result = await categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category
            {
                CategoryId = 1,
                Name = "Electronics"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(category);

            var categoryService = new CategoryService(
                categoryRepositoryMock.Object);

            // Act
            var result = await categoryService.GetCategoryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CategoryId);
            Assert.Equal("Electronics", result.Name);
        }


        [Fact]
        public async Task CreateCategoryAsync_ReturnsCreatedCategory()
        {
            // Arrange
            var category = new Category
            {
                CategoryId = 3,
                Name = "Home Appliances"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.AddAsync(category))
                .ReturnsAsync(category);

            var categoryService = new CategoryService(
                categoryRepositoryMock.Object);

            // Act
            var result = await categoryService.CreateCategoryAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.CategoryId);
            Assert.Equal("Home Appliances", result.Name);

            categoryRepositoryMock.Verify(
                repo => repo.AddAsync(category),
                Times.Once);
        }


        [Fact]
        public async Task UpdateCategoryAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var category = new Category
            {
                CategoryId = 1,
                Name = "Updated Electronics"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var categoryService = new CategoryService(
                categoryRepositoryMock.Object);

            // Act
            await categoryService.UpdateCategoryAsync(category);

            // Assert
            categoryRepositoryMock.Verify(
                repo => repo.UpdateAsync(category),
                Times.Once);
        }


        [Fact]
        public async Task DeleteCategoryAsync_CallsRepositoryDelete()
        {
            // Arrange
            int categoryId = 1;

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var categoryService = new CategoryService(
                categoryRepositoryMock.Object);

            // Act
            await categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            categoryRepositoryMock.Verify(
                repo => repo.DeleteAsync(categoryId),
                Times.Once);
        }
    }
}