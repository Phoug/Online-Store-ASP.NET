using Shared.DTO.Wishlist;
using Shared.Models;
using Shared.Repositories;
using Shared.Mappers;
using System.Linq;
using System.Collections.Generic;

namespace Shared.Services
{
    public class WishlistServiceImpl : IWishlistService
    {
        private readonly IWishlistRepository _repository;

        public WishlistServiceImpl(IWishlistRepository repository)
        {
            _repository = repository;
        }

        public async Task<WishlistReadDto?> GetByIdAsync(int id)
        {
            var wishlist = await _repository.GetByIdAsync(id);
            if (wishlist == null) return null;

            return WishlistMapper.ToReadDto(wishlist);
        }

        public async Task<WishlistDetailedDto?> GetDetailedAsync(int id)
        {
            var wishlist = await _repository.GetByIdAsync(id);
            if (wishlist == null) return null;

            return WishlistMapper.ToDetailedDto(wishlist);
        }

        public async Task<WishlistReadDto?> GetByUserIdAsync(int userId)
        {
            var wishlist = await _repository.GetByUserIdAsync(userId);
            return wishlist == null ? null : WishlistMapper.ToReadDto(wishlist);
        }

        public async Task<IEnumerable<WishlistShortDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(WishlistMapper.ToShortDto).ToList();
        }

        public async Task<WishlistReadDto> CreateAsync(WishlistCreateDto dto)
        {
            var entity = new Wishlist
            {
                UserId = dto.UserId,
                // Не инициализируем вложенный User полностью (во избежание рекурсивных required-инициализаций).
                // Достаточно задать FK; навигационные свойства установит EF при трекинге/загрузке.
                User = null!,
                WishlistItems = new List<WishlistItem>()
            };

            // Используем возвращаемое значение репозитория и сохраняем изменения
            var created = await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();

            return WishlistMapper.ToReadDto(created);
        }

        public async Task<WishlistReadDto?> UpdateAsync(int id, WishlistUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            // Частичное обновление: если dto.UserId == null — оставляем текущее значение
            if (dto.UserId.HasValue)
                entity.UserId = dto.UserId.Value;

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return WishlistMapper.ToReadDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}