using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP_NET.Shared.Models;

namespace Shared.Repositories
{
    public class WishlistRepositoryImpl : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?> GetByIdAsync(int id)
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Wishlist?> GetByUserIdAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.WishlistItems)
                .ToListAsync();
        }

        public async Task<Wishlist> CreateAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await SaveChangesAsync();
            return wishlist;
        }

        public async Task UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Wishlists.FindAsync(id);
            if (entity != null)
            {
                _context.Wishlists.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
