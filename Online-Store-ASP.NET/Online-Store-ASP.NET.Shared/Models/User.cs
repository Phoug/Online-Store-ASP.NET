using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, Username, Name, Password, Email, Phone, Role,
    /// RegistrationDate, BirthDate, CartId, Cart, WishlistId,
    /// Wishlist, ReviewsList, OrdersList, DeliveriesList
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier (primary key).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Login name used for authentication.
        /// </summary>
        [MaxLength(64)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the user.
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for secure authentication.
        /// </summary>
        [MaxLength(64)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Role of the user in the system (e.g., Admin, Customer).
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Guest;

        /// <summary>
        /// Date and time when the user registered.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Year of birth of the user.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Navigation property to the Cart.
        /// One-to-one relationship with Cart.
        /// </summary>
        public required virtual Cart Cart { get; set; }

        /// <summary>
        /// Navigation property to the Wishlist.
        /// One-to-one relationship with Wishlist.
        /// </summary>
        public required virtual Wishlist Wishlist { get; set; }

        /// <summary>
        /// User's reviews.
        /// One-to-many relationship: one User can write many reviews.
        /// </summary>
        public virtual ICollection<Review> ReviewsList { get; set; } = new List<Review>();

        /// <summary>
        /// User's orders.
        /// One-to-many relationship: one User can have many orders.
        /// </summary>
        public virtual ICollection<Order> OrdersList { get; set; } = new List<Order>();

        /// <summary>
        /// Navigation property to the deliveries associated with the user.
        /// One-to-many relationship: one User can have many Deliveries.
        /// </summary>
        public virtual ICollection<Delivery> DeliveriesList { get; set; } = new List<Delivery>();
    }

    /// <summary>
    /// Defines possible roles for a user.
    /// </summary>
    public enum UserRole
    {
        Admin,
        Manager,
        Customer,
        Guest
    }
}
