using Shared.DTO.Wishlist;

namespace Shared.Services
{
    public interface IWishlistService
    {
        Task<WishlistReadDto?> GetByIdAsync(int id);
        Task<WishlistDetailedDto?> GetDetailedAsync(int id);
        Task<WishlistReadDto?> GetByUserIdAsync(int userId);
        Task<IEnumerable<WishlistShortDto>> GetAllAsync();

        Task<WishlistReadDto> CreateAsync(WishlistCreateDto dto);
        Task<WishlistReadDto?> UpdateAsync(int id, WishlistUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
