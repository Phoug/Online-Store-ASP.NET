using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Infrastructure.Data;

namespace Shared.Repositories
{
    public class WishlistItemRepositoryImpl : IWishlistItemRepository
    {
        private readonly AppDbContext _context;

        public WishlistItemRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistItem>> GetAllAsync()
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .Include(wi => wi.Wishlist)
                .ToListAsync();
        }

        public async Task<WishlistItem?> GetByIdAsync(int id)
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .Include(wi => wi.Wishlist)
                .FirstOrDefaultAsync(wi => wi.Id == id);
        }

        public async Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(int wishlistId)
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .Where(wi => wi.WishlistId == wishlistId)
                .ToListAsync();
        }

        public async Task AddAsync(WishlistItem item)
        {
            await _context.WishlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WishlistItem item)
        {
            _context.WishlistItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.WishlistItems.FindAsync(id);
            if (entity != null)
            {
                _context.WishlistItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}