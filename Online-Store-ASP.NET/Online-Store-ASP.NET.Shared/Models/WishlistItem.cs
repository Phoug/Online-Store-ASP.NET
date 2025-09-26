using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, WishlistId, Wishlist, ProductId, Product, AddedAt
    /// </summary>
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Wishlist.
        /// </summary>
        [Required]
        public int WishlistId { get; set; }

        /// <summary>
        /// Wishlist that contains this product.
        /// One-to-many relationship: one Wishlist can have many WishlistProducts.
        /// </summary>
        [ForeignKey("WishlistId")]
        public virtual Wishlist Wishlist { get; set; } = null!;

        /// <summary>
        /// Foreign key to the Product.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation property to the Product.
        /// One-to-many relationship: one Product can appear in many WishlistProducts.
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
