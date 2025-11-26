using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class ProductRepositoryImpl : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.ReviewsList)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.ReviewsList)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
