namespace Online_Store_ASP_NET.Shared.DTO.Product
{
    /// <summary>
    /// Детальное DTO товара, включает категории, отзывы и рейтинг.
    /// </summary>
    public class ProductDetailsDto
    {
        public int Id { get; set; }

        public string Article { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public List<string> MediaUrls { get; set; } = new();

        /// <summary>
        /// Категории, к которым относится товар.
        /// </summary>
        public List<ProductCategoryDto> Categories { get; set; } = new();

        /// <summary>
        /// Отзывы о товаре (сокращенная версия).
        /// </summary>
        public List<ProductReviewShortDto> Reviews { get; set; } = new();

        /// <summary>
        /// Средний рейтинг товара.
        /// </summary>
        public double AverageRating { get; set; }
    }
}
