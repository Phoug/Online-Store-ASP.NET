using System;

namespace Shared.DTO.WishlistItem
{
    public class WishlistItemReadDto
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public Shared.DTO.Wishlist.WishlistShortDto Wishlist { get; set; } = null!;

        public int ProductId { get; set; }

        public Shared.DTO.Product.ProductReadDto Product { get; set; } = null!;
    }
}