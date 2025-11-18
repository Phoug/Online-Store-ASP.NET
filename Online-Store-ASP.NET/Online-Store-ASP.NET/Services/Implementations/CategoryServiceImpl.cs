using Services.Interfaces;
using Shared.Models;
using Repositories.Interfaces;

namespace Services.Implementations
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryServiceImpl(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _repository.AddAsync(category);
        }

        public async Task UpdateAsync(int id, Category category)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Name = category.Name;
            existing.Description = category.Description;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
