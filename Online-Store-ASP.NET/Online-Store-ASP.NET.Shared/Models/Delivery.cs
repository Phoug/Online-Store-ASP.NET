namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, Address, DeliveryMethod, DeliveryCost, StartDate, EndDate, OrderId, Order, UserId, User
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Unique identifier for the delivery.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Delivery address.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Delivery method (e.g., Courier, PickupPoint, Express).
        /// </summary>
        public string DeliveryMethod { get; set; } = string.Empty;

        /// <summary>
        /// Cost of delivery.
        /// </summary>
        public decimal DeliveryCost { get; set; }

        /// <summary>
        /// Planned start date of delivery (e.g., when shipped).
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Planned or actual end date of delivery (e.g., when delivered).
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Foreign key to the related order.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation property to the related order.
        /// One-to-one relationship: one Order has one Delivery.
        /// </summary>
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Optional foreign key to the user who receives the delivery.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Navigation property to the user (customer).
        /// One-to-many relationship: one User can have many Deliveries.
        /// </summary>
        public User? User { get; set; }
    }
}
