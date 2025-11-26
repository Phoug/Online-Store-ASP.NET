using Shared.Models;
using Shared.Repositories;

namespace Shared.Services
{
    public class WishlistItemServiceImpl : IWishlistItemService
    {
        private readonly IWishlistItemRepository _repository;

        public WishlistItemServiceImpl(IWishlistItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WishlistItem>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<WishlistItem?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(int wishlistId) =>
            await _repository.GetByWishlistIdAsync(wishlistId);

        public async Task<WishlistItem> AddAsync(WishlistItem item)
        {
            // Ensure AddedAt if not provided
            if (item.AddedAt == default) item.AddedAt = DateTime.UtcNow;

            await _repository.AddAsync(item);
            await _repository.SaveChangesAsync();
            return item;
        }

        public async Task<WishlistItem?> UpdateAsync(int id, WishlistItem item)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            // Обновляем только релевантные поля
            existing.ProductId = item.ProductId;
            existing.WishlistId = item.WishlistId;
            existing.AddedAt = item.AddedAt;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}