using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Review
{
    /// <summary>
    /// DTO для создания нового отзыва.
    /// </summary>
    public class CreateReviewDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
