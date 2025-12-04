using Moq;
using FluentAssertions;
using Services.Implementations;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IOrderItemRepository> _mockRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly OrderItemService _service;

        public OrderItemServiceTests()
        {
            _mockRepository = new Mock<IOrderItemRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _service = new OrderItemService(
                _mockRepository.Object,
                _mockProductRepository.Object,
                _mockOrderRepository.Object);
        }

        private OrderItem CreateValidOrderItem(int id, int quantity = 1)
        {
            return new OrderItem
            {
                Id = id,
                Quantity = quantity,
                OrderId = 10,
                ProductId = 20,
                Order = new Order
                {
                    Id = 10,
                    Status = "Pending",
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Username = "testuser",
                        Cart = new Cart { Id = 1 },
                        Wishlist = new Wishlist { Id = 1 }
                    }
                },
                Product = new Product
                {
                    Id = 20,
                    Name = "Test Product",
                    Article = "TEST-001",
                    Price = 99.99m
                }
            };
        }

        [Fact]
        public async Task GetAsync_WhenOrderItemExists_ShouldReturnOrderItem()
        {
            var expectedOrderItem = CreateValidOrderItem(1, 2);
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedOrderItem);

            var result = await _service.GetAsync(1);

            result.Should().BeEquivalentTo(expectedOrderItem);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetAsync_WhenOrderItemNotExists_ShouldReturnNull()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OrderItem?)null);

            var result = await _service.GetAsync(999);

            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrderItems()
        {
            var expectedOrderItems = new List<OrderItem>
            {
                CreateValidOrderItem(1, 3),
                CreateValidOrderItem(2, 1)
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedOrderItems);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(expectedOrderItems);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_WhenOrderAndProductExist_ShouldCreateOrderItem()
        {
            var orderItemToCreate = CreateValidOrderItem(1, 5);
            var existingOrder = new Order
            {
                Id = 10,
                Status = "Pending",
                UserId = 1,
                User = new User
                {
                    Id = 1,
                    Username = "testuser",
                    Cart = new Cart { Id = 1 },
                    Wishlist = new Wishlist { Id = 1 }
                }
            };
            var existingProduct = new Product
            {
                Id = 20,
                Name = "Test Product",
                Article = "TEST-001",
                Price = 99.99m
            };

            _mockOrderRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(existingOrder);
            _mockProductRepository.Setup(r => r.GetByIdAsync(20)).ReturnsAsync(existingProduct);
            _mockRepository.Setup(r => r.AddAsync(orderItemToCreate)).ReturnsAsync(orderItemToCreate);

            var result = await _service.CreateAsync(orderItemToCreate);

            result.Should().BeEquivalentTo(orderItemToCreate);
            _mockOrderRepository.Verify(r => r.GetByIdAsync(10), Times.Once());
            _mockProductRepository.Verify(r => r.GetByIdAsync(20), Times.Once());
            _mockRepository.Verify(r => r.AddAsync(orderItemToCreate), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_WhenOrderNotExists_ShouldThrowArgumentException()
        {
            var orderItemToCreate = CreateValidOrderItem(1, 1);
            orderItemToCreate.OrderId = 999;

            _mockOrderRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateAsync(orderItemToCreate));

            exception.Message.Should().Contain("Order with ID 999 not found.");
            _mockOrderRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockProductRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never());
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Never());
        }

        [Fact]
        public async Task CreateAsync_WhenProductNotExists_ShouldThrowArgumentException()
        {
            var orderItemToCreate = CreateValidOrderItem(1, 1);
            orderItemToCreate.ProductId = 999;
            var existingOrder = new Order
            {
                Id = 10,
                Status = "Pending",
                UserId = 1,
                User = new User
                {
                    Id = 1,
                    Username = "testuser",
                    Cart = new Cart { Id = 1 },
                    Wishlist = new Wishlist { Id = 1 }
                }
            };

            _mockOrderRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(existingOrder);
            _mockProductRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateAsync(orderItemToCreate));

            exception.Message.Should().Contain("Product with ID 999 not found.");
            _mockOrderRepository.Verify(r => r.GetByIdAsync(10), Times.Once());
            _mockProductRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Never());
        }

        [Fact]
        public async Task UpdateAsync_WhenOrderItemExists_ShouldUpdateProperties()
        {
            var existingOrderItem = CreateValidOrderItem(1, 2);
            var updatedOrderItem = CreateValidOrderItem(1, 5);
            updatedOrderItem.OrderId = 15;
            updatedOrderItem.ProductId = 25;

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingOrderItem);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<OrderItem>())).ReturnsAsync(existingOrderItem);

            var result = await _service.UpdateAsync(1, updatedOrderItem);

            existingOrderItem.Quantity.Should().Be(5);
            existingOrderItem.OrderId.Should().Be(15);
            existingOrderItem.ProductId.Should().Be(25);
            result.Should().Be(existingOrderItem);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(existingOrderItem), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenOrderItemNotExists_ShouldReturnNull()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OrderItem?)null);

            var result = await _service.UpdateAsync(999, CreateValidOrderItem(999));

            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<OrderItem>()), Times.Never());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse()
        {
            _mockRepository.Setup(r => r.DeleteAsync(2)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(2);

            result.Should().BeFalse();
            _mockRepository.Verify(r => r.DeleteAsync(2), Times.Once());
        }

        [Fact]
        public void Constructor_ShouldAssignDependencies()
        {
            var repository = new Mock<IOrderItemRepository>().Object;
            var productRepo = new Mock<IProductRepository>().Object;
            var orderRepo = new Mock<IOrderRepository>().Object;
            var service = new OrderItemService(repository, productRepo, orderRepo);

            service.Should().NotBeNull();
        }
    }
}
