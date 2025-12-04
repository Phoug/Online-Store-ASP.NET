// ProductControllerTests.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _serviceMock = new();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _controller = new ProductController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithList()
    {
        // Arrange
        var products = new List<Product> { CreateValidProduct(1) };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(products, ok.Value);
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetProductById_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetProductById_Existing_ReturnsOk()
    {
        // Arrange
        var product = CreateValidProduct(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(product, ok.Value);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_Null_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.CreateProduct(null!);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Product object cannot be null.", bad.Value);
        _serviceMock.Verify(s => s.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task CreateProduct_Valid_ReturnsCreated()
    {
        // Arrange
        var product = CreateValidProduct(1);

        // Act
        var result = await _controller.CreateProduct(product);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ProductController.GetProductById), created.ActionName);
        Assert.Same(product, created.Value);
        _serviceMock.Verify(s => s.AddAsync(product), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var product = CreateValidProduct(2);

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product ID mismatch.", bad.Value);
        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProduct_NotFound_ReturnsNotFound()
    {
        // Arrange
        var product = CreateValidProduct(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_Existing_ReturnsNoContent()
    {
        // Arrange
        var product = CreateValidProduct(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    private static Product CreateValidProduct(int id)
    {
        return new Product
        {
            Id = id,
            Article = $"ART{id}",
            Name = "Test Product",
            Description = "Test product description",
            Price = 100m,
            MediaUrls = new List<string> { "https://example.com/image.jpg" },
            Categories = new List<Category>(),
            OrderItems = new List<OrderItem>(),
            CartItems = new List<CartItem>(),
            WishlistItems = new List<WishlistItem>(),
            ReviewsList = new List<Review>()
        };
    }
}
