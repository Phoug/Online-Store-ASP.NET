namespace Shared.DTO.Review
{
    /// <summary>
    /// Краткое описание товара для вывода в отзыв.
    /// </summary>
    public class ReviewProductPreviewDto
    {
        public int Id { get; set; }

        public string Article { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? PreviewImageUrl { get; set; }
    }
}
