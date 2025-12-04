using Online_Store_ASP_NET.Shared.Models;

namespace Shared.Repositories
{
    public interface IWishlistItemRepository
    {
        Task<IEnumerable<WishlistItem>> GetAllAsync();
        Task<WishlistItem?> GetByIdAsync(int id);
        Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(int wishlistId);

        Task AddAsync(WishlistItem item);
        Task UpdateAsync(WishlistItem item);
        Task DeleteAsync(int id);

        Task SaveChangesAsync();
    }
}