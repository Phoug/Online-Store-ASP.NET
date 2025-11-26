using Shared.DTO.Product;
using Shared.DTO.Wishlist;

namespace Shared.DTO.WishlistItem
{
    public class WishlistItemDetailedDto
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }

        public WishlistShortDto Wishlist { get; set; } = null!;

        public int ProductId { get; set; }

        public ProductDetailsDto Product { get; set; } = null!;
    }
}