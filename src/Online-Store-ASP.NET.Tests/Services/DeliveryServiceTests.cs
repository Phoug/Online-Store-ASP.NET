using Moq;
using FluentAssertions;
using Services.Implementations;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class DeliveryServiceImplTests
    {
        private readonly Mock<IDeliveryRepository> _mockRepository;
        private readonly DeliveryServiceImpl _service;

        public DeliveryServiceImplTests()
        {
            _mockRepository = new Mock<IDeliveryRepository>();
            _service = new DeliveryServiceImpl(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDeliveries()
        {
            // Arrange
            var expectedDeliveries = new List<Delivery>
            {
                new Delivery { Id = 1, UserId = 1 },
                new Delivery { Id = 2, UserId = 2 }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedDeliveries);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedDeliveries);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenDeliveryExists_ShouldReturnDelivery()
        {
            // Arrange
            var expectedDelivery = new Delivery
            {
                Id = 1,
                Address = "123 Main St",
                DeliveryMethod = "Standard"
            };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedDelivery);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(expectedDelivery);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenDeliveryNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Delivery?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnUserDeliveries()
        {
            // Arrange
            var expectedDeliveries = new List<Delivery>
            {
                new Delivery { Id = 1, UserId = 42 },
                new Delivery { Id = 2, UserId = 42 }
            };
            _mockRepository.Setup(r => r.GetByUserIdAsync(42)).ReturnsAsync(expectedDeliveries);

            // Act
            var result = await _service.GetByUserIdAsync(42);

            // Assert
            result.Should().BeEquivalentTo(expectedDeliveries);
            _mockRepository.Verify(r => r.GetByUserIdAsync(42), Times.Once());
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var deliveryToAdd = new Delivery
            {
                Id = 3,
                Address = "456 Oak Ave",
                DeliveryCost = 10.99m
            };

            // Act
            await _service.AddAsync(deliveryToAdd);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(deliveryToAdd), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenDeliveryExists_ShouldUpdatePropertiesAndCallRepository()
        {
            // Arrange
            var existingDelivery = new Delivery
            {
                Id = 1,
                Address = "Old Address",
                DeliveryMethod = "Old Method",
                DeliveryCost = 5.00m,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };
            var updatedDelivery = new Delivery
            {
                Id = 1,
                Address = "New Address",
                DeliveryMethod = "Express",
                DeliveryCost = 15.99m,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                OrderId = 123,
                UserId = 42
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingDelivery);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Delivery>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updatedDelivery);

            // Assert
            existingDelivery.Address.Should().Be("New Address");
            existingDelivery.DeliveryMethod.Should().Be("Express");
            existingDelivery.DeliveryCost.Should().Be(15.99m);
            existingDelivery.StartDate.Should().Be(updatedDelivery.StartDate);
            existingDelivery.EndDate.Should().Be(updatedDelivery.EndDate);
            existingDelivery.OrderId.Should().Be(123);
            existingDelivery.UserId.Should().Be(42);

            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(existingDelivery), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenDeliveryNotExists_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Delivery?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateAsync(999, new Delivery()));

            exception.Message.Should().Contain("Delivery with ID 999 not found.");
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>()), Times.Never());
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
            var repository = new Mock<IDeliveryRepository>().Object;
            var service = new DeliveryServiceImpl(repository);

            // Assert
            service.Should().NotBeNull();
        }
    }
}
