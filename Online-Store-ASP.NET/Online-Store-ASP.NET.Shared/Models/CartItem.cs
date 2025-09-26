using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    ///  Fields: Id, Quantity, TotalPrice, CartId, Cart, ProductId, Product
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Primary key for the CartProduct.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Quantity of this product in the cart.
        /// </summary>
        [Required]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Calculates the total price for this item (Quantity * Product Price)
        /// </summary>
        [NotMapped]
        public decimal TotalPrice => Product != null ? Quantity * Product.Price : 0;

        /// <summary>
        /// Foreign key to the Cart.
        /// </summary>
        [Required]
        public int CartId { get; set; }

        /// <summary>
        /// Navigation property to the Cart.
        /// One-to-many relationship: one Cart can have many CartItems.
        /// </summary>
        [ForeignKey("CartId")]
        public required virtual Cart Cart { get; set; }

        /// <summary>
        /// Foreign key to the Product.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation property to the Product.
        /// One-to-many relationship: one Product can appear in many CartItems.
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = new Product();
    }
}
