using Shared.DTO.User;
using Shared.DTO.WishlistItem;
using System.Collections.Generic;

namespace Shared.DTO.Wishlist
{
    public class WishlistReadDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserShortDto User { get; set; } = null!;

        public List<WishlistItemReadDto> WishlistItems { get; set; } = new();

        public int ItemsCount { get; set; }
    }
}