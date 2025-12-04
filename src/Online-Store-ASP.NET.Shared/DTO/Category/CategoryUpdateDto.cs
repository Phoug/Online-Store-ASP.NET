using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.Category
{
    /// <summary>
    /// DTO для обновления категории.
    /// </summary>
    public class CategoryUpdateDto
    {
        /// <summary>
        /// Новое название категории.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Новое описание категории.
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
    }
}
