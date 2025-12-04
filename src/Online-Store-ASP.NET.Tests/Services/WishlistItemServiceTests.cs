// WishlistControllerTests.cs - ЕДИНСТВЕННЫЙ РАБОЧИЙ ФАЙЛ
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Shared.DTO.Wishlist;
using Shared.Services;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class WishlistControllerTests
{
    private readonly Mock<IWishlistService> _mockService;
    private readonly WishlistController _controller;

    public WishlistControllerTests()
    {
        _mockService = new Mock<IWishlistService>();
        _controller = new WishlistController(_mockService.Object);
    }

    [Fact]
    public async Task Get_NotFound_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((WishlistReadDto?)null);
        var result = await _controller.Get(1);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_Existing_ReturnsOk()
    {
        var dto = new WishlistReadDto { Id = 1, UserId = 1 };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        var result = await _controller.Get(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(dto, ok.Value);
    }

    [Fact]
    public async Task GetDetailed_NotFound_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetDetailedAsync(1)).ReturnsAsync((WishlistDetailedDto?)null);
        var result = await _controller.GetDetailed(1);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetDetailed_Existing_ReturnsOk()
    {
        var dto = new WishlistDetailedDto { Id = 1, UserId = 1 };
        _mockService.Setup(s => s.GetDetailedAsync(1)).ReturnsAsync(dto);
        var result = await _controller.GetDetailed(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(dto, ok.Value);
    }

    [Fact]
    public async Task GetByUserId_NotFound_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync((WishlistReadDto?)null);
        var result = await _controller.GetByUserId(1);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetByUserId_Existing_ReturnsOk()
    {
        var dto = new WishlistReadDto { Id = 1, UserId = 1 };
        _mockService.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync(dto);
        var result = await _controller.GetByUserId(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(dto, ok.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var createDto = new WishlistCreateDto { UserId = 1 };
        var readDto = new WishlistReadDto { Id = 10, UserId = 1 };
        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);
        var result = await _controller.Create(createDto);
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(WishlistController.Get), created.ActionName);
    }

    [Fact]
    public async Task Update_NotFound_ReturnsNotFound()
    {
        var updateDto = new WishlistUpdateDto { UserId = 2 };
        _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync((WishlistReadDto?)null);
        var result = await _controller.Update(1, updateDto);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_Success_ReturnsNoContent()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
    }
}
