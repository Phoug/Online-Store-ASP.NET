namespace Shared.DTO.Product
{
    namespace Shared.DTO.Category
    {
        /// <summary>
        /// DTO для отображения информации о категории.
        /// Может включать список продуктов в сокращённом виде.
        /// </summary>
        public class CategoryReadDto
        {
            /// <summary>
            /// Уникальный идентификатор категории.
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
            /// Список продуктов (облегчённое отображение).
            /// </summary>
            public List<ProductListItemDto> Products { get; set; } = new();
        }
    }
}
