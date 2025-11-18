using Shared.DTO.User;
using Shared.DTO.WishlistItem;
using Shared.DTOs.WishlistItem;
using System.Collections.Generic;

namespace Shared.DTO.Wishlist
{
    public class WishlistDetailedDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserDetailedDto User { get; set; } = null!;

        public List<WishlistItemDetailedDto> WishlistItems { get; set; } = new();

        // Для маппера — список DTO элементов (короткий формат)
        public List<WishlistItemDto> Items { get; set; } = new();
    }
}