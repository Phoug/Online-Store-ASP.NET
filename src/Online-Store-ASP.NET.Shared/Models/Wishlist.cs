using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    /// <summary>
    /// Fields: Id, UserId, User, WishlistItems
    /// </summary>
    public class Wishlist
    {
        /// <summary>
        /// Primary key for the wishlist.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the User who owns this wishlist.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the User who owns this wishlist.
        /// </summary>
        public required virtual User User { get; set; }

        /// <summary>
        /// Collection of products in the wishlist.
        /// One-to-Many
        /// </summary>
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
}