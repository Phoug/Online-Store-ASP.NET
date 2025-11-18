using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Shared.Models;

namespace Repositories.Implementations
{
    /// <summary>
    /// Implementation of the repository for managing Delivery entities using Entity Framework Core.
    /// </summary>
    public class DeliveryRepositoryImpl : IDeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<Delivery?> GetByIdAsync(int id)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Delivery>> GetByUserIdAsync(int userId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Deliveries.FindAsync(id);
            if (entity != null)
            {
                _context.Deliveries.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
