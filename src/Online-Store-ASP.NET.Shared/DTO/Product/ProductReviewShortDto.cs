namespace Shared.DTO.Product
{
    /// <summary>
    /// Короткое DTO отзыва, используемое внутри ProductDetailsDto.
    /// </summary>
    public class ProductReviewShortDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Оценка от 1 до 5.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Краткий текст отзыва.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Имя автора.
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
