namespace Online_Store_ASP_NET.Shared.DTO.Category
{
    /// <summary>
    /// Краткая информация о категории.
    /// Применяется в списках, выпадающих меню и т.п.
    /// </summary>
    public class CategoryListItemDto
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
