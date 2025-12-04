namespace Online_Store_ASP_NET.Shared.DTO.Product
{
    /// <summary>
    /// Краткое DTO продукта — используется в списках.
    /// </summary>
    public class ProductListItemDto
    {
        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL главного изображения (если есть).
        /// </summary>
        public string? PreviewImage { get; set; }
    }
}
