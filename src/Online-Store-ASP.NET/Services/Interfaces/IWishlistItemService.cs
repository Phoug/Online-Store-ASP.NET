using Online_Store_ASP_NET.Shared.Models;

namespace Shared.Services
{
    public interface IWishlistItemService
    {
        Task<IEnumerable<WishlistItem>> GetAllAsync();
        Task<WishlistItem?> GetByIdAsync(int id);
        Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(int wishlistId);

        Task<WishlistItem> AddAsync(WishlistItem item);
        Task<WishlistItem?> UpdateAsync(int id, WishlistItem item);
        Task<bool> DeleteAsync(int id);
    }
}