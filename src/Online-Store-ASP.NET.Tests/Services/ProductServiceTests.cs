using Moq;
using FluentAssertions;
using Services.Implementations;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class ProductServiceImplTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductServiceImpl _service;

        public ProductServiceImplTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductServiceImpl(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var expectedProduct = new Product
            {
                Id = 1,
                Name = "iPhone 15",
                Price = 899.99m,
                Article = "IPH15-128"
            };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(expectedProduct);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

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
            var productToAdd = new Product
            {
                Id = 3,
                Name = "Headphones",
                Price = 199.99m
            };

            // Act
            await _service.AddAsync(productToAdd);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(productToAdd), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenIdMatches_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var productToUpdate = new Product
            {
                Id = 1,
                Name = "Updated Laptop",
                Price = 1099.99m
            };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, productToUpdate);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(productToUpdate), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenIdMismatch_ShouldThrowArgumentException()
        {
            // Arrange
            var productWithWrongId = new Product { Id = 2, Name = "Wrong ID Product" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdateAsync(1, productWithWrongId));

            exception.Message.Should().Be("Product ID mismatch.");
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never());
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
            var repository = new Mock<IProductRepository>().Object;
            var service = new ProductServiceImpl(repository);

            // Assert
            service.Should().NotBeNull();
        }
    }
}
