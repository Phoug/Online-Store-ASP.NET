using Moq;
using FluentAssertions;
using Shared.Services;
using Shared.DTO.Wishlist;
using Online_Store_ASP_NET.Shared.Models;
using Shared.Repositories;
using Shared.Mappers;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class WishlistServiceImplTests
    {
        private readonly Mock<IWishlistRepository> _mockRepository;
        private readonly WishlistServiceImpl _service;

        public WishlistServiceImplTests()
        {
            _mockRepository = new Mock<IWishlistRepository>();
            _service = new WishlistServiceImpl(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WhenWishlistExists_ShouldReturnReadDto()
        {
            // Arrange
            var wishlist = new Wishlist { Id = 1, UserId = 10 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(wishlist);
            WishlistMapper.ToReadDto(wishlist).Should().NotBeNull();

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenWishlistNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Wishlist?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDetailedAsync_WhenWishlistExists_ShouldReturnDetailedDto()
        {
            // Arrange
            var wishlist = new Wishlist { Id = 1, UserId = 10 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(wishlist);
            WishlistMapper.ToDetailedDto(wishlist).Should().NotBeNull();

            // Act
            var result = await _service.GetDetailedAsync(1);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetDetailedAsync_WhenWishlistNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Wishlist?)null);

            // Act
            var result = await _service.GetDetailedAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenWishlistExists_ShouldReturnReadDto()
        {
            // Arrange
            var wishlist = new Wishlist { Id = 1, UserId = 10 };
            _mockRepository.Setup(r => r.GetByUserIdAsync(10)).ReturnsAsync(wishlist);
            WishlistMapper.ToReadDto(wishlist).Should().NotBeNull();

            // Act
            var result = await _service.GetByUserIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.Verify(r => r.GetByUserIdAsync(10), Times.Once());
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenWishlistNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByUserIdAsync(999)).ReturnsAsync((Wishlist?)null);

            // Act
            var result = await _service.GetByUserIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllShortDtos()
        {
            // Arrange
            var wishlists = new List<Wishlist>
            {
                new Wishlist { Id = 1, UserId = 10 },
                new Wishlist { Id = 2, UserId = 20 }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(wishlists);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnReadDto()
        {
            // Arrange
            var dto = new WishlistCreateDto { UserId = 10 };
            var entity = new Wishlist { Id = 1, UserId = 10 };
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Wishlist>())).ReturnsAsync(entity);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(dto.UserId);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Wishlist>()), Times.Once());
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenWishlistExists_ShouldUpdateAndReturnReadDto()
        {
            // Arrange
            var existing = new Wishlist { Id = 1, UserId = 10 };
            var dto = new WishlistUpdateDto { UserId = 20 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepository.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(1, dto);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(dto.UserId);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
            _mockRepository.Verify(r => r.UpdateAsync(existing), Times.Once());
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenWishlistNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Wishlist?)null);

            // Act
            var result = await _service.UpdateAsync(999, new WishlistUpdateDto());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenWishlistExists_ShouldDeleteAndReturnTrue()
        {
            // Arrange
            var entity = new Wishlist { Id = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once());
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_WhenWishlistNotExists_ShouldReturnFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Wishlist?)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}
