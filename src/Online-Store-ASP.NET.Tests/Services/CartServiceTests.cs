using Moq;
using FluentAssertions;
using Services.Implementations;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class CartServiceImplTests
    {
        private readonly Mock<ICartRepository> _mockRepository;
        private readonly CartServiceImpl _service;

        public CartServiceImplTests()
        {
            _mockRepository = new Mock<ICartRepository>();
            _service = new CartServiceImpl(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCarts()
        {
            // Arrange
            var expectedCarts = new List<Cart>
            {
                new Cart { Id = 1, UserId = 1 },
                new Cart { Id = 2, UserId = 2 }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedCarts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedCarts);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenCartExists_ShouldReturnCart()
        {
            // Arrange
            var expectedCart = new Cart { Id = 1, UserId = 10 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedCart);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(expectedCart);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenCartNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Cart?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnUserCart()
        {
            // Arrange
            var expectedCart = new Cart { Id = 5, UserId = 42 };
            _mockRepository.Setup(r => r.GetByUserIdAsync(42)).ReturnsAsync(expectedCart);

            // Act
            var result = await _service.GetByUserIdAsync(42);

            // Assert
            result.Should().BeEquivalentTo(expectedCart);
            _mockRepository.Verify(r => r.GetByUserIdAsync(42), Times.Once());
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var cartToAdd = new Cart { Id = 3, UserId = 3 };

            // Act
            await _service.AddAsync(cartToAdd);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(cartToAdd), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenCartExists_ShouldUpdatePropertiesAndCallRepository()
        {
            // Arrange
            var existingCart = new Cart { Id = 1, UserId = 10 };
            var updatedCart = new Cart { Id = 1, UserId = 20 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCart);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Cart>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updatedCart);

            // Assert
            existingCart.UserId.Should().Be(20);
            existingCart.CartItems.Should().BeEquivalentTo(updatedCart.CartItems);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(existingCart), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenCartNotExists_ShouldNotUpdate()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Cart?)null);

            // Act
            await _service.UpdateAsync(999, new Cart());

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Cart>()), Times.Never());
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
            var repository = new Mock<ICartRepository>().Object;
            var service = new CartServiceImpl(repository);

            // Assert
            service.Should().NotBeNull();
        }
    }
}
