using Online_Store_ASP_NET.Shared.Models;

namespace Services.Interfaces
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId);
        Task AddAsync(CartItem item);
        Task UpdateAsync(int id, CartItem item);
        Task DeleteAsync(int id);
    }
}
