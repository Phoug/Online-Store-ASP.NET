using Online_Store_ASP_NET.Shared.Models;

namespace Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(int id);
        Task<Cart?> GetByUserIdAsync(int userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(int id, Cart cart);
        Task DeleteAsync(int id);
    }
}
