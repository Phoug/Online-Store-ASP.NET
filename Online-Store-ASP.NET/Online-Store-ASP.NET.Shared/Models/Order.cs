using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, TotalAmount, Status, CreatedAt, UpdatedAt, UserId, User, DeliveryId, Delivery, OrderProducts
    public class Order
    {
        /// <summary>
        /// Primary key for the Order.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Total amount of the order. Can be calculated dynamically.
        /// </summary>
        [NotMapped]
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                foreach (var item in OrderProducts)
                {
                    total += item.Quantity * item.Product.Price;
                }
                return total;
            }
        }

        /// <summary>
        /// Status of the order (e.g., Pending, Shipped, Delivered).
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Date when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the order was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Foreign key to the User who placed the order.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the User who placed the order.
        /// </summary>
        [ForeignKey("UserId")]
        public required virtual User User { get; set; }

        /// <summary>
        /// Foreign key to the Delivery details for this order.
        /// </summary>.
        public int? DeliveryId { get; set; }

        /// <summary>
        /// Related delivery details for this order.
        /// One-to-one relationship: one Order has one Delivery.
        /// </summary>
        public virtual Delivery? Delivery { get; set; }

        /// <summary>
        /// Collection of order items in this order.
        /// One-to-many relationship: one Order can have many OrderItems.
        /// </summary>
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];
    }
}