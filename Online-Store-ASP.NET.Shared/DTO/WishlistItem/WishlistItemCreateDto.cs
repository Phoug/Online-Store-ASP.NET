using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.WishlistItem
{
    public class WishlistItemCreateDto
    {
        [Required]
        public int WishlistId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
