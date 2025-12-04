// OrderItemControllerTests.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class OrderItemControllerTests
{
    private readonly Mock<IOrderItemService> _serviceMock = new();
    private readonly OrderItemController _controller;

    public OrderItemControllerTests()
    {
        _controller = new OrderItemController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetOrderItems_ReturnsOk()
    {
        // Arrange
        var list = new List<OrderItem> { CreateValidOrderItem(1) };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        // Act
        var result = await _controller.GetOrderItems();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(list, ok.Value);
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetOrderItem_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync((OrderItem?)null);

        // Act
        var result = await _controller.GetOrderItem(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetOrderItem_Existing_ReturnsOk()
    {
        // Arrange
        var item = CreateValidOrderItem(1);
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(item);

        // Act
        var result = await _controller.GetOrderItem(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(item, ok.Value);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateOrderItem_Null_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.CreateOrderItem(null!);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("OrderItem cannot be null.", bad.Value);
        _serviceMock.Verify(s => s.CreateAsync(It.IsAny<OrderItem>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderItem_Valid_ReturnsCreated()
    {
        // Arrange
        var item = CreateValidOrderItem(1);

        // Act
        var result = await _controller.CreateOrderItem(item);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(OrderItemController.GetOrderItem), created.ActionName);
        Assert.Same(item, created.Value);
        _serviceMock.Verify(s => s.CreateAsync(item), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderItem_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var item = CreateValidOrderItem(2);

        // Act
        var result = await _controller.UpdateOrderItem(1, item);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID mismatch.", bad.Value);
        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<OrderItem>()), Times.Never);
    }

    [Fact]
    public async Task UpdateOrderItem_NotFound_ReturnsNotFound()
    {
        // Arrange
        var item = CreateValidOrderItem(1);
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync((OrderItem?)null);

        // Act
        var result = await _controller.UpdateOrderItem(1, item);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderItem_Existing_ReturnsNoContent()
    {
        // Arrange
        var item = CreateValidOrderItem(1);
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(item);

        // Act
        var result = await _controller.UpdateOrderItem(1, item);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
        _serviceMock.Verify(s => s.UpdateAsync(1, item), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderItem_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync((OrderItem?)null);

        // Act
        var result = await _controller.DeleteOrderItem(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderItem_Existing_ReturnsNoContent()
    {
        // Arrange
        var item = CreateValidOrderItem(1);
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(item);

        // Act
        var result = await _controller.DeleteOrderItem(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.GetAsync(1), Times.Once);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    private static OrderItem CreateValidOrderItem(int id)
    {
        // Create User (required for Order)
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Name = "Test User",
            Email = "test@example.com",
            ReviewsList = new List<Review>(),
            OrdersList = new List<Order>(),
            DeliveriesList = new List<Delivery>()
        };

        // Create Order with required User
        var order = new Order
        {
            Id = 1,
            Status = "Pending",
            UserId = user.Id,
            User = user, // Required navigation property
            DeliveryId = 1,
            OrderItems = new List<OrderItem>()
        };

        // Create Product with correct collections (not CategoryId)
        var product = new Product
        {
            Id = 1,
            Article = "ART001",
            Name = "Test Product",
            Description = "Test description",
            Price = 10m,
            MediaUrls = new List<string>(),
            Categories = new List<Category>(),
            OrderItems = new List<OrderItem>(),
            CartItems = new List<CartItem>(),
            WishlistItems = new List<WishlistItem>(),
            ReviewsList = new List<Review>()
        };

        // Create OrderItem
        var item = new OrderItem
        {
            Id = id,
            OrderId = order.Id,
            ProductId = product.Id,
            Quantity = 1,
            Order = order,
            Product = product
        };

        // Link back to collections
        order.OrderItems.Add(item);
        product.OrderItems.Add(item);

        return item;
    }
}
