using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, Quantity, TotalPrice, OrderId, Order, ProductId, Product
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Primary key for the OrderItem.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Quantity of this product in the order.
        /// </summary>
        [Required]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Total price for this order item (Quantity * Product Price).
        /// </summary>
        [NotMapped]
        public decimal TotalPrice => Product != null ? Quantity * Product.Price : 0;

        /// <summary>
        /// Foreign key to the Order.
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation property to the Order.
        /// One-to-many relationship: one Order can have many OrderItems.
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        /// <summary>
        /// Foreign key to the Product.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation property to the Product.
        /// One-to-many relationship: one Product can appear in many OrderItems.
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
