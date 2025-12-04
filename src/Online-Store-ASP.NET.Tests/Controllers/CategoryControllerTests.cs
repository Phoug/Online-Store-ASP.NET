// CategoryControllerTests.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _serviceMock = new();
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _controller = new CategoryController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsOk()
    {
        var list = new List<Category> { CreateValidCategory(1) };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        var result = await _controller.GetCategories();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(list, ok.Value);
    }

    [Fact]
    public async Task GetCategoryById_NotFound_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Category?)null);

        var result = await _controller.GetCategoryById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCategory_Null_ReturnsBadRequest()
    {
        var result = await _controller.CreateCategory(null!);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateCategory_Valid_ReturnsCreated()
    {
        var cat = CreateValidCategory(1);

        var result = await _controller.CreateCategory(cat);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CategoryController.GetCategoryById), created.ActionName);
        Assert.Same(cat, created.Value);
    }

    [Fact]
    public async Task UpdateCategory_IdMismatch_ReturnsBadRequest()
    {
        var cat = CreateValidCategory(2);

        var result = await _controller.UpdateCategory(1, cat);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCategory_NotFound_ReturnsNotFound()
    {
        var cat = CreateValidCategory(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Category?)null);

        var result = await _controller.UpdateCategory(1, cat);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_Existing_ReturnsNoContent()
    {
        var cat = CreateValidCategory(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cat);

        var result = await _controller.DeleteCategory(1);

        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    private static Category CreateValidCategory(int id) =>
        new()
        {
            Id = id,
            Name = "Category"
        };
}
