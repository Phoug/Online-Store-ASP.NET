using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.Product
{
    /// <summary>
    /// DTO для обновления информации о товаре.
    /// </summary>
    public class ProductUpdateDto
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Полный список медиа (перезаписывается).
        /// </summary>
        public List<string> MediaUrls { get; set; } = new();

        /// <summary>
        /// Новые категории товара.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new();
    }
}
