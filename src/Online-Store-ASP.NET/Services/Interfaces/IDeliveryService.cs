using Online_Store_ASP_NET.Shared.Models;

namespace Services.Interfaces
{
    /// <summary>
    /// Service interface for Delivery entity operations.
    /// </summary>
    public interface IDeliveryService
    {
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery?> GetByIdAsync(int id);
        Task<IEnumerable<Delivery>> GetByUserIdAsync(int userId);
        Task AddAsync(Delivery delivery);
        Task UpdateAsync(int id, Delivery delivery);
        Task DeleteAsync(int id);
    }
}
