namespace Shared.DTO.Review
{
    /// <summary>
    /// DTO отображения отзыва.
    /// </summary>
    public class ReviewReadDto
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int AuthorId { get; set; }

        public ReviewAuthorDto Author { get; set; } = new();

        public int ProductId { get; set; }

        public ReviewProductPreviewDto Product { get; set; } = new();
    }
}
