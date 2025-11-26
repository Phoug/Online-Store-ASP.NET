using Shared.Models;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<IEnumerable<OrderItem>> FindAsync(Expression<Func<OrderItem, bool>> predicate);
        Task<OrderItem> AddAsync(OrderItem entity);
        Task<OrderItem> UpdateAsync(OrderItem entity);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}