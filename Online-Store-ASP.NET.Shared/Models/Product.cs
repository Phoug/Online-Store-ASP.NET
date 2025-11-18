using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    /// <summary>
    /// Fields: Id, Article, Name, Description, Price, MediaUrls, Categories, OrderProducts, CartProducts, WishlistProducts, ReviewsList
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier (primary key).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Unique article code for convenient search.
        /// </summary>
        [MaxLength(11)]
        public string Article { get; set; } = string.Empty;

        /// <summary>
        /// Name of the product.
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the product.
        /// </summary>
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// List of URLs to product photos or videos.
        /// </summary>
        public List<string> MediaUrls { get; set; } = [];

        /// <summary>
        /// Collection of categories this product belongs to.
        /// Many-to-many relationship between Product and Category.
        /// </summary>
        public virtual ICollection<Category>? Categories { get; set; } = [];

        /// <summary>
        /// Collection of order items that include this product.
        /// One-to-many relationship: one Product can appear in many OrderItems.
        /// </summary>
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = [];

        /// <summary>
        /// Collection of cart items that include this product.
        /// One-to-many relationship: one Product can appear in many CartItems.
        /// </summary>
        public virtual ICollection<CartItem>? CartItems { get; set; } = [];

        /// <summary>
        /// Collection of wishlist items that include this product.
        /// One-to-many relationship: one Product can appear in many WishlistItems.
        /// </summary>
        public virtual ICollection<WishlistItem>? WishlistItems { get; set; } = [];

        /// <summary>
        /// Collection of reviews written for this product.
        /// One-to-many relationship: one Product can have many reviews.
        /// </summary>
        public virtual ICollection<Review>? ReviewsList { get; set; } = [];

    }

}
