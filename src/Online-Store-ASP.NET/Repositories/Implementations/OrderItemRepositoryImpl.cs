using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using System.Linq.Expressions;


namespace Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<OrderItem> _dbSet;

        public OrderItemRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<OrderItem>();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _dbSet
            .Include(x => x.Product)
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _dbSet
            .Include(x => x.Product)
            .Include(x => x.Order)
            .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> FindAsync(Expression<Func<OrderItem, bool>> predicate)
        {
            return await _dbSet
            .Include(x => x.Product)
            .Include(x => x.Order)
            .Where(predicate)
            .ToListAsync();
        }

        public async Task<OrderItem> AddAsync(OrderItem entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<OrderItem> UpdateAsync(OrderItem entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}