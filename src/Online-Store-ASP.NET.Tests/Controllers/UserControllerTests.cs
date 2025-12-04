// UserControllerTests.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.DTO.User;
using Services.UserService;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _serviceMock = new();
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _controller = new UserController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOk()
    {
        var list = new List<UserReadDto> { new() { Id = 1, Username = "test" } };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        var result = await _controller.GetAllUsers();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(list, ok.Value);
    }

    [Fact]
    public async Task GetUserById_InvalidId_ReturnsBadRequest()
    {
        var result = await _controller.GetUserById(0);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_NotFound_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((UserReadDto?)null);

        var result = await _controller.GetUserById(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserByUsername_Empty_ReturnsBadRequest()
    {
        var result = await _controller.GetUserByUsername("  ");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task RegisterUser_Null_ReturnsBadRequest()
    {
        var result = await _controller.RegisterUser(null!);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task RegisterUser_ServiceThrows_Returns500()
    {
        var dto = new UserCreateDto { Username = "user", Email = "a@a.a", Password = "123456" };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("error"));

        var result = await _controller.RegisterUser(dto);

        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, obj.StatusCode);
    }
}
