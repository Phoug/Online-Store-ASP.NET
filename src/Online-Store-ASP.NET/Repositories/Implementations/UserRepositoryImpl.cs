using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP_NET.Shared.Models;

namespace Shared.Repositories
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Cart)
                .Include(u => u.Wishlist)
                .Include(u => u.ReviewsList)
                .Include(u => u.OrdersList)
                .Include(u => u.DeliveriesList)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .Include(u => u.Wishlist)
                .Include(u => u.ReviewsList)
                .Include(u => u.OrdersList)
                .Include(u => u.DeliveriesList)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .Include(u => u.Wishlist)
                .Include(u => u.ReviewsList)
                .Include(u => u.OrdersList)
                .Include(u => u.DeliveriesList)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity != null)
            {
                _context.Users.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}