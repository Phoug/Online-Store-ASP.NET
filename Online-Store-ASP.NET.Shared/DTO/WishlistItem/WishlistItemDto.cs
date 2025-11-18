using System;

namespace Shared.DTO.WishlistItem
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }

        // Добавлено поле времени добавления
        public DateTime AddedAt { get; set; }
    }
}