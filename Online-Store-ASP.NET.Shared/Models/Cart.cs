using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    /// <summary>
    /// Fields: Id, TotalPrice, UserId, User, CartProducts
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Primary key for the Cart.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Total price of all items in the cart. Can be calculated dynamically.
        /// </summary>
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                if (CartItems != null)
                {
                    foreach (var item in CartItems)
                    {
                        if (item?.Product != null)
                            total += item.Quantity * item.Product.Price;
                    }
                }
                return total;
            }
        }

        /// <summary>
        /// Foreign key to the User who owns this cart.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the User who owns this cart.
        /// One-to-one relationship: one User has one Cart.
        /// (Не обязательное для инициализации поле — избегаем требования object-initializer.)
        /// </summary>
        public virtual User? User { get; set; } = null!;

        /// <summary>
        /// Collection of items in the cart.
        /// One-to-many relationship: one Cart can have many CartItems.
        /// </summary>
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}