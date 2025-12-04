// OrderControllerTests.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _serviceMock = new();
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _controller = new OrderController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetOrders_ReturnsOk()
    {
        var list = new List<Order> { CreateValidOrder(1) };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        var result = await _controller.GetOrders();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(list, ok.Value);
    }

    [Fact]
    public async Task GetOrderById_NotFound_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Order?)null);

        var result = await _controller.GetOrderById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetOrdersByUserId_Empty_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync(Enumerable.Empty<Order>());

        var result = await _controller.GetOrdersByUserId(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateOrder_Null_ReturnsBadRequest()
    {
        var result = await _controller.CreateOrder(null!);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateOrder_IdMismatch_ReturnsBadRequest()
    {
        var order = CreateValidOrder(2);

        var result = await _controller.UpdateOrder(1, order);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_Existing_ReturnsNoContent()
    {
        var order = CreateValidOrder(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(order);

        var result = await _controller.DeleteOrder(1);

        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    private static Order CreateValidOrder(int id)
    {
        var user = new User
        {
            Id = 1,
            Username = "test",
            Email = "test@test.com"
        };

        var delivery = new Delivery
        {
            Id = 1,
            Address = "Test",
            DeliveryMethod = "Курьер",
            DeliveryCost = 100,
            UserId = user.Id,
            OrderId = id
        };

        var order = new Order
        {
            Id = id,
            Status = "Pending",
            UserId = user.Id,
            DeliveryId = delivery.Id,
            User = user,
            Delivery = delivery,
            OrderItems = new List<OrderItem>()
        };

        return order;
    }
}
