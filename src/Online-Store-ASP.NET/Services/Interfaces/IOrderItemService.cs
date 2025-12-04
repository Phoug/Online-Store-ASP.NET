using Online_Store_ASP_NET.Shared.Models;

namespace Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItem?> GetAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> CreateAsync(OrderItem model);
        Task<OrderItem?> UpdateAsync(int id, OrderItem model);
        Task<bool> DeleteAsync(int id);
    }
}