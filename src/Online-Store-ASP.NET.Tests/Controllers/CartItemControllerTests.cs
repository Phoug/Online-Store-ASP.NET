using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Controllers;

namespace Online_Store_ASP_NET.Tests.Controllers
{
    public class CartItemControllerTests
    {
        private readonly Mock<ICartItemService> _mockService;
        private readonly CartItemController _controller;

        public CartItemControllerTests()
        {
            _mockService = new Mock<ICartItemService>();
            _controller = new CartItemController(_mockService.Object);
        }

        private CartItem CreateValidCartItem(int id, int cartId, int productId, int quantity = 1)
        {
            return new CartItem
            {
                Id = id,
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,
                Cart = new Cart
                {
                    Id = cartId,
                    UserId = 1,
                    User = new User { Id = 1, Username = "testuser" }
                },
                Product = new Product { Id = productId, Name = "Test Product" }
            };
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithAllCartItems()
        {
            var items = new List<CartItem>
            {
                CreateValidCartItem(1, 1, 1, 2),
                CreateValidCartItem(2, 1, 2, 3)
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(items);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetById_ItemExists_ReturnsOk()
        {
            var item = CreateValidCartItem(1, 1, 1, 2);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(item);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetById_ItemNotExists_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((CartItem?)null);

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCartId_ItemsExist_ReturnsOk()
        {
            var items = new List<CartItem>
            {
                CreateValidCartItem(1, 1, 1, 2)
            };
            _mockService.Setup(s => s.GetByCartIdAsync(1)).ReturnsAsync(items);

            var result = await _controller.GetByCartId(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetByCartId_NoItems_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByCartIdAsync(2)).ReturnsAsync(new List<CartItem>());

            var result = await _controller.GetByCartId(2);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidCartItem_ReturnsCreated()
        {
            var item = CreateValidCartItem(5, 3, 12, 2);
            _mockService.Setup(s => s.AddAsync(item)).Returns(Task.CompletedTask);

            var result = await _controller.Create(item);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues!["id"].Should().Be(item.Id);
            createdResult.Value.Should().Be(item);
        }

        [Fact]
        public async Task Create_NullCartItem_ReturnsBadRequest()
        {
            var result = await _controller.Create(null!);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.Value.Should().Be("CartItem object cannot be null.");
        }

        [Fact]
        public async Task Create_InvalidCartItem_ReturnsBadRequest()
        {
            var item = new CartItem
            {
                Id = 0,
                CartId = 0,
                ProductId = 0,
                Quantity = 1,
                Cart = new Cart { Id = 0 },
                Product = new Product { Id = 0 }
            };

            var result = await _controller.Create(item);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.Value.Should().Be("Both CartId and ProductId must be specified.");
        }

        [Fact]
        public async Task Update_Valid_ReturnsNoContent()
        {
            var item = CreateValidCartItem(7, 3, 12, 4);
            _mockService.Setup(s => s.GetByIdAsync(7)).ReturnsAsync(item);
            _mockService.Setup(s => s.UpdateAsync(7, item)).Returns(Task.CompletedTask);

            var result = await _controller.Update(7, item);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var item = CreateValidCartItem(8, 3, 12, 4);

            var result = await _controller.Update(7, item);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("CartItem ID mismatch or body is null.");
        }

        [Fact]
        public async Task Update_ItemNotFound_ReturnsNotFound()
        {
            var item = CreateValidCartItem(5, 3, 12, 4);
            _mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((CartItem?)null);

            var result = await _controller.Update(5, item);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ItemExists_ReturnsNoContent()
        {
            var item = CreateValidCartItem(7, 3, 12, 2);
            _mockService.Setup(s => s.GetByIdAsync(7)).ReturnsAsync(item);
            _mockService.Setup(s => s.DeleteAsync(7)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(7);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ItemNotExists_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((CartItem?)null);

            var result = await _controller.Delete(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
