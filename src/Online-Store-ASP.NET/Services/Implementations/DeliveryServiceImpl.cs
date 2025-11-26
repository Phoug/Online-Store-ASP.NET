using Repositories.Interfaces;
using Services.Interfaces;
using Shared.Models;

namespace Services.Implementations
{
    public class DeliveryServiceImpl : IDeliveryService
    {
        private readonly IDeliveryRepository _repository;

        public DeliveryServiceImpl(IDeliveryRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Delivery>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Delivery?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<IEnumerable<Delivery>> GetByUserIdAsync(int userId) => _repository.GetByUserIdAsync(userId);

        public Task AddAsync(Delivery delivery) => _repository.AddAsync(delivery);

        public async Task UpdateAsync(int id, Delivery delivery)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Delivery with ID {id} not found.");

            existing.Address = delivery.Address;
            existing.DeliveryMethod = delivery.DeliveryMethod;
            existing.DeliveryCost = delivery.DeliveryCost;
            existing.StartDate = delivery.StartDate;
            existing.EndDate = delivery.EndDate;
            existing.OrderId = delivery.OrderId;
            existing.UserId = delivery.UserId;

            await _repository.UpdateAsync(existing);
        }

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
