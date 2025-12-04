using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.Category
{
    /// <summary>
    /// DTO для создания новой категории.
    /// Включает обязательные поля.
    /// </summary>
    public class CategoryCreateDto
    {
        /// <summary>
        /// Название категории.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание категории.
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
    }
}
