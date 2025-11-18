using Shared.DTO.Product;

namespace Shared.DTO.Category
{
    /// <summary>
    /// DTO для расширенного вывода категории
    /// вместе с полным набором данных о продуктах.
    /// </summary>
    public class CategoryWithProductsDto
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание категории.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Продукты данной категории — в виде полного набора DTO.
        /// </summary>
        public List<ProductReadDto> Products { get; set; } = new();
    }
}
