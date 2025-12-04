using Moq;
using FluentAssertions;
using Services.Implementations;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class CategoryServiceImplTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly CategoryServiceImpl _service;

        public CategoryServiceImplTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _service = new CategoryServiceImpl(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedCategories);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedCategories);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategory()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Id = 1,
                Name = "Books",
                Description = "All types of books"
            };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedCategory);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(expectedCategory);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var categoryToAdd = new Category
            {
                Id = 3,
                Name = "Sports",
                Description = "Sporting goods"
            };

            // Act
            await _service.AddAsync(categoryToAdd);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(categoryToAdd), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryExists_ShouldUpdatePropertiesAndCallRepository()
        {
            // Arrange
            var existingCategory = new Category
            {
                Id = 1,
                Name = "Old Name",
                Description = "Old Description"
            };
            var updatedCategory = new Category
            {
                Id = 1,
                Name = "New Name",
                Description = "New Description"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCategory);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updatedCategory);

            // Assert
            existingCategory.Name.Should().Be("New Name");
            existingCategory.Description.Should().Be("New Description");
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(existingCategory), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryNotExists_ShouldNotUpdate()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);

            // Act
            await _service.UpdateAsync(999, new Category());

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never());
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(5);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(5), Times.Once());
        }

        [Fact]
        public void Constructor_ShouldAssignRepository()
        {
            // Arrange
            var repository = new Mock<ICategoryRepository>().Object;
            var service = new CategoryServiceImpl(repository);

            // Assert
            service.Should().NotBeNull();
        }
    }
}
