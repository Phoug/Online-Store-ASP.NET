using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Review
{
    /// <summary>
    /// DTO для изменения существующего отзыва.
    /// </summary>
    public class UpdateReviewDto
    {
        [Range(1, 5)]
        public int? Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
}
