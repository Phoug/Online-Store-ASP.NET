using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, UserId, User, WishlistProducts
    /// </summary>
    public class Wishlist
    {
        /// <summary>
        /// Primary key for the wishlist.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the User who owns this wishlist.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the User who owns this wishlist.
        /// </summary>
        [ForeignKey("UserId")]
        public required virtual User User { get; set; }

        /// <summary>
        /// Collection of products in the wishlist.
        /// Many-to-many relationship via WishlistProduct.
        /// </summary>
        public virtual ICollection<WishlistProduct> WishlistProducts { get; set; } = new List<WishlistProduct>();
    }
}