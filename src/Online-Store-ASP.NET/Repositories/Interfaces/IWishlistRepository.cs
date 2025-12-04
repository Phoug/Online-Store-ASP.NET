using Online_Store_ASP_NET.Shared.Models;

namespace Shared.Repositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByIdAsync(int id);
        Task<Wishlist?> GetByUserIdAsync(int userId);
        Task<IEnumerable<Wishlist>> GetAllAsync();

        Task<Wishlist> CreateAsync(Wishlist wishlist);
        Task UpdateAsync(Wishlist wishlist);
        Task DeleteAsync(int id);

        Task SaveChangesAsync();
    }
}
