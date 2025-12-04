using Services.Interfaces;
using Repositories.Interfaces;
using Online_Store_ASP_NET.Shared.Models;

namespace Services
{
    public class ReviewServiceImpl : IReviewService
    {
        private readonly IReviewRepository _repository;

        public ReviewServiceImpl(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Review?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
            => await _repository.GetByProductIdAsync(productId);

        public async Task AddAsync(Review review)
            => await _repository.AddAsync(review);

        public async Task UpdateAsync(int id, Review updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Rating = updated.Rating;
            existing.Comment = updated.Comment;
            existing.ProductId = updated.ProductId;
            existing.AuthorId = updated.AuthorId;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
