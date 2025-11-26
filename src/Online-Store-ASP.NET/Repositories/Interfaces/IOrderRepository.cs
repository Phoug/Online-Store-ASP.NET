using Shared.Models;

namespace Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
