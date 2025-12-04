using Repositories.Interfaces;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;

namespace Services.Implementations
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderServiceImpl(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task AddAsync(Order order)
        {
            await _repository.AddAsync(order);
        }

        public async Task UpdateAsync(int id, Order order)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Заказ с ID = {id} не найден.");

            existing.Status = order.Status;
            existing.DeliveryId = order.DeliveryId;
            existing.OrderItems = order.OrderItems;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Заказ с ID = {id} не найден.");

            await _repository.DeleteAsync(existing);
        }
    }
}
