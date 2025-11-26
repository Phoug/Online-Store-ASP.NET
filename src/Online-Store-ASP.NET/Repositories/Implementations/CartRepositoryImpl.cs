using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Repositories.Interfaces;
using Infrastructure.Data;

namespace Repositories.Implementations
{
    /// <summary>
    /// Реализация репозитория для корзин.
    /// </summary>
    public class CartRepositoryImpl : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Carts.FindAsync(id);
            if (entity != null)
            {
                _context.Carts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
