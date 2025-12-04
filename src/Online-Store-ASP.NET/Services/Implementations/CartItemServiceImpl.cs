using Services.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;

namespace Services.Implementations
{
    public class CartItemServiceImpl : ICartItemService
    {
        private readonly ICartItemRepository _repository;

        public CartItemServiceImpl(ICartItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _repository.GetByCartIdAsync(cartId);
        }

        public async Task AddAsync(CartItem item)
        {
            await _repository.AddAsync(item);
        }

        public async Task UpdateAsync(int id, CartItem item)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Quantity = item.Quantity;
            existing.ProductId = item.ProductId;
            existing.CartId = item.CartId;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
