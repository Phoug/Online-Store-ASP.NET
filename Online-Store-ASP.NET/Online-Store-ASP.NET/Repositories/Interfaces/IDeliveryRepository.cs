using Shared.Models;

namespace Repositories.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery?> GetByIdAsync(int id);
        Task<IEnumerable<Delivery>> GetByUserIdAsync(int userId);
        Task AddAsync(Delivery delivery);
        Task UpdateAsync(Delivery delivery);
        Task DeleteAsync(int id);
    }
}
