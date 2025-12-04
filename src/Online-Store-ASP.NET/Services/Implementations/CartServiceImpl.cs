using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;

namespace Services.Implementations
{
    /// <summary>
    /// Реализация сервисного слоя для корзин.
    /// </summary>
    public class CartServiceImpl : ICartService
    {
        private readonly ICartRepository _repository;

        public CartServiceImpl(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _repository.AddAsync(cart);
        }

        public async Task UpdateAsync(int id, Cart cart)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.UserId = cart.UserId;
            existing.CartItems = cart.CartItems;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
