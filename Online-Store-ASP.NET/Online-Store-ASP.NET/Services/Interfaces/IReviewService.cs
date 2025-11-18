using Shared.Models;

namespace Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task AddAsync(Review review);
        Task UpdateAsync(int id, Review updated);
        Task DeleteAsync(int id);
    }
}
