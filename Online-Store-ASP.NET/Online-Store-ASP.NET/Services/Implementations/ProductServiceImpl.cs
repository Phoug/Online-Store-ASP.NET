using Shared.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductServiceImpl(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(Product product)
        {
            return _repository.AddAsync(product);
        }

        public async Task UpdateAsync(int id, Product product)
        {
            if (id != product.Id)
                throw new ArgumentException("Product ID mismatch.");

            await _repository.UpdateAsync(product);
        }

        public Task DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
