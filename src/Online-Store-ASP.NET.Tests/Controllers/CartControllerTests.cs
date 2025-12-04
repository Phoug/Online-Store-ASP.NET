using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Controllers;

namespace Online_Store_ASP_NET.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockService;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockService = new Mock<ICartService>();
            _controller = new CartController(_mockService.Object);
        }

        [Fact]
        public async Task GetCarts_ReturnsOkWithCarts()
        {
            // Arrange
            var carts = new List<Cart>
            {
                new Cart { Id = 1, UserId = 1 },
                new Cart { Id = 2, UserId = 2 }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(carts);

            // Act
            var result = await _controller.GetCarts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(carts);
        }

        [Fact]
        public async Task GetCartById_CartExists_ReturnsOk()
        {
            // Arrange
            var cart = new Cart { Id = 1, UserId = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cart);

            // Act
            var result = await _controller.GetCartById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(cart);
        }

        [Fact]
        public async Task GetCartById_CartNotExists_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Cart?)null);

            var result = await _controller.GetCartById(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCartByUserId_CartExists_ReturnsOk()
        {
            var cart = new Cart { Id = 1, UserId = 1 };
            _mockService.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync(cart);

            var result = await _controller.GetCartByUserId(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(cart);
        }

        [Fact]
        public async Task GetCartByUserId_CartNotExists_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByUserIdAsync(999)).ReturnsAsync((Cart?)null);

            var result = await _controller.GetCartByUserId(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCart_ValidCart_ReturnsCreatedAtAction()
        {
            var cart = new Cart { Id = 1, UserId = 1 };

            _mockService.Setup(s => s.AddAsync(cart)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCart(cart);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            createdResult.ActionName.Should().Be(nameof(_controller.GetCartById));
            createdResult.RouteValues!["id"].Should().Be(cart.Id);
            createdResult.Value.Should().Be(cart);
        }

        [Fact]
        public async Task CreateCart_NullCart_ReturnsBadRequest()
        {
            var result = await _controller.CreateCart(null!);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.Value.Should().Be("Cart cannot be null.");
        }

        [Fact]
        public async Task UpdateCart_Valid_UpdateReturnsNoContent()
        {
            var cart = new Cart { Id = 1, UserId = 1 };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cart);
            _mockService.Setup(s => s.UpdateAsync(1, cart)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCart(1, cart);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCart_IdMismatch_ReturnsBadRequest()
        {
            var cart = new Cart { Id = 2, UserId = 1 };

            var result = await _controller.UpdateCart(1, cart);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Cart ID mismatch.");
        }

        [Fact]
        public async Task UpdateCart_NotFound_ReturnsNotFound()
        {
            var cart = new Cart { Id = 1, UserId = 1 };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Cart?)null);

            var result = await _controller.UpdateCart(1, cart);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCart_Found_ReturnsNoContent()
        {
            var cart = new Cart { Id = 1, UserId = 1 };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cart);
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCart(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCart_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Cart?)null);

            var result = await _controller.DeleteCart(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
