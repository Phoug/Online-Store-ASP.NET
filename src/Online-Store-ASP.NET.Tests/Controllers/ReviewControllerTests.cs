// ReviewControllerTests.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Controllers;

public class ReviewControllerTests
{
    private readonly Mock<IReviewService> _serviceMock = new();
    private readonly ReviewController _controller;

    public ReviewControllerTests()
    {
        _controller = new ReviewController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetReviews_ReturnsOk()
    {
        // Arrange
        var list = new List<Review> { CreateValidReview(1) };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        // Act
        var result = await _controller.GetReviews();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(list, ok.Value);
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetReview_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Review?)null);

        // Act
        var result = await _controller.GetReview(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetReview_Existing_ReturnsOk()
    {
        // Arrange
        var review = CreateValidReview(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(review);

        // Act
        var result = await _controller.GetReview(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(review, ok.Value);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateReview_Null_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.CreateReview(null!);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Review object cannot be null.", bad.Value);
        _serviceMock.Verify(s => s.AddAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task CreateReview_InvalidRating_ReturnsBadRequest()
    {
        // Arrange
        var review = CreateValidReview(0);
        review.Rating = 0; // Invalid rating (should be 1-5)

        // Act
        var result = await _controller.CreateReview(review);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Rating must be from 1 to 5.", bad.Value);
        _serviceMock.Verify(s => s.AddAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task CreateReview_Valid_ReturnsCreated()
    {
        // Arrange
        var review = CreateValidReview(1);

        // Act
        var result = await _controller.CreateReview(review);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ReviewController.GetReview), created.ActionName);
        Assert.Same(review, created.Value);
        _serviceMock.Verify(s => s.AddAsync(review), Times.Once);
    }

    [Fact]
    public async Task UpdateReview_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var review = CreateValidReview(2);

        // Act
        var result = await _controller.UpdateReview(1, review);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID mismatch.", bad.Value);
        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task UpdateReview_NotFound_ReturnsNotFound()
    {
        // Arrange
        var review = CreateValidReview(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Review?)null);

        // Act
        var result = await _controller.UpdateReview(1, review);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task UpdateReview_Existing_ReturnsNoContent()
    {
        // Arrange
        var review = CreateValidReview(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(review);

        // Act
        var result = await _controller.UpdateReview(1, review);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.UpdateAsync(1, review), Times.Once);
    }

    [Fact]
    public async Task DeleteReview_NotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Review?)null);

        // Act
        var result = await _controller.DeleteReview(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteReview_Existing_ReturnsNoContent()
    {
        // Arrange
        var review = CreateValidReview(1);
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(review);

        // Act
        var result = await _controller.DeleteReview(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByProduct_ReturnsOk()
    {
        // Arrange
        var productId = 1;
        var reviews = new List<Review> { CreateValidReview(1) };
        _serviceMock.Setup(s => s.GetByProductIdAsync(productId)).ReturnsAsync(reviews);

        // Act
        var result = await _controller.GetByProduct(productId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(reviews, ok.Value);
        _serviceMock.Verify(s => s.GetByProductIdAsync(productId), Times.Once);
    }

    private static Review CreateValidReview(int id)
    {
        Product product = new Product
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

        User author = new User
        {
            Id = 1,
            Username = "testuser",
            Name = "Test User",
            Email = "test@example.com",
            ReviewsList = new List<Review>(),
            OrdersList = new List<Order>(),
            DeliveriesList = new List<Delivery>()
        };

        return new Review
        {
            Id = id,
            Rating = 5,
            Comment = "Great product!",
            AuthorId = author.Id,
            Author = author,
            ProductId = product.Id,
            Product = product
        };
    }
}
