using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.WishlistItem
{
    public class WishlistItemUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int WishlistId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
