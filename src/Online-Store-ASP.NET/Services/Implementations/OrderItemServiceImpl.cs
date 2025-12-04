using Services.Interfaces;
using Repositories.Interfaces;
using Online_Store_ASP_NET.Shared.Models;

namespace Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderItemService(
            IOrderItemRepository repository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderItem?> GetAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderItem> CreateAsync(OrderItem model)
        {
            var order = await _orderRepository.GetByIdAsync(model.OrderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {model.OrderId} not found.");

            var product = await _productRepository.GetByIdAsync(model.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {model.ProductId} not found.");

            return await _repository.AddAsync(model);
        }

        public async Task<OrderItem?> UpdateAsync(int id, OrderItem model)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return null;

            existing.Quantity = model.Quantity;
            existing.ProductId = model.ProductId;
            existing.OrderId = model.OrderId;

            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
