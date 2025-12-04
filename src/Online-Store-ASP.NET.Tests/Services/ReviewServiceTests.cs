using Moq;
using FluentAssertions;
using Services;
using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class ReviewServiceImplTests
    {
        private readonly Mock<IReviewRepository> _mockRepository;
        private readonly ReviewServiceImpl _service;

        public ReviewServiceImplTests()
        {
            _mockRepository = new Mock<IReviewRepository>();
            _service = new ReviewServiceImpl(_mockRepository.Object);
        }

        private Review CreateValidReview(int id, int rating, string comment)
        {
            return new Review
            {
                Id = id,
                Rating = rating,
                Comment = comment,
                ProductId = 1,
                AuthorId = 1,
                Product = new Product { Id = 1, Name = "Test Product" }, // Обязательное
                Author = new User { Id = 1, Username = "testuser" }      // Обязательное
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllReviews()
        {
            var expectedReviews = new List<Review>
            {
                CreateValidReview(1, 5, "Excellent!"),
                CreateValidReview(2, 4, "Good product")
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedReviews);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(expectedReviews);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenReviewExists_ShouldReturnReview()
        {
            var expectedReview = CreateValidReview(1, 5, "Amazing!");
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedReview);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeEquivalentTo(expectedReview);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }
    }
}
