using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP_NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, Rating, Comment, CreatedAt, AuthorId, Author, ProductId, Product
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unique identifier (primary key).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Rating given in the review (e.g., from 1 to 5).
        /// </summary>
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// Textual comment of the review.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Date when the review was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Foreign key to the User who authored the review.
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// Navigation property to the User who authored the review.
        /// </summary>
        [ForeignKey("AuthorId")]
        public required virtual User Author { get; set; }

        /// <summary>
        /// Foreign key to the Product being reviewed.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation property to the Product being reviewed.
        /// </summary>
        [ForeignKey("ProductId")]
        public required virtual Product Product { get; set; }
    }
}
