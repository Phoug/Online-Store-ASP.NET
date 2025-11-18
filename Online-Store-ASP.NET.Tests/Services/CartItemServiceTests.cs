using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Services.Implementations;
using Repositories.Interfaces;
using Shared.Models;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class CartItemServiceImplTests
    {
        private readonly Mock<ICartItemRepository> _repositoryMock;
        private readonly CartItemServiceImpl _service;

        public CartItemServiceImplTests()
        {
            _repositoryMock = new Mock<ICartItemRepository>();
            _service = new CartItemServiceImpl(_repositoryMock.Object);
        }

        private CartItem CreateItem(int id = 1)
        {
            return new CartItem
            {
                Id = id,
                Quantity = 1,
                ProductId = 1,
                CartId = 1,
                Cart = new Cart()
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            var items = new List<CartItem>
            {
                CreateItem(1),
                CreateItem(2)
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
        {
            var item = CreateItem(1);

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((CartItem?)null);

            var result = await _service.GetByIdAsync(100);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByCartIdAsync_ShouldReturnItems()
        {
            var items = new List<CartItem>
            {
                CreateItem(1),
                CreateItem(2)
            };

            _repositoryMock.Setup(r => r.GetByCartIdAsync(10)).ReturnsAsync(items);

            var result = await _service.GetByCartIdAsync(10);

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepository()
        {
            var item = CreateItem();

            await _service.AddAsync(item);

            _repositoryMock.Verify(r => r.AddAsync(item), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem_WhenFound()
        {
            var existing = CreateItem(1);

            var updated = new CartItem
            {
                Quantity = 8,
                ProductId = 20,
                CartId = 30,
                Cart = new Cart()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            await _service.UpdateAsync(1, updated);

            existing.Quantity.Should().Be(8);
            existing.ProductId.Should().Be(20);
            existing.CartId.Should().Be(30);

            _repositoryMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldDoNothing_WhenItemNotFound()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((CartItem?)null);

            var updated = new CartItem
            {
                Quantity = 10,
                ProductId = 2,
                CartId = 1,
                Cart = new Cart()
            };

            await _service.UpdateAsync(999, updated);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            await _service.DeleteAsync(5);

            _repositoryMock.Verify(r => r.DeleteAsync(5), Times.Once);
        }
    }
}
