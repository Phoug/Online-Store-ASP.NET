using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Repositories.Interfaces;
using Infrastructure.Data;

namespace Repositories.Implementations
{
    public class CartItemRepositoryImpl : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task AddAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CartItems.FindAsync(id);
            if (entity != null)
            {
                _context.CartItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
