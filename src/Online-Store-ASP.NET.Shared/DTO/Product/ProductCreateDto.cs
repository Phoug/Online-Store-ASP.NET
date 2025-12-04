using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.Product
{
    /// <summary>
    /// DTO для создания нового товара.
    /// </summary>
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(11)]
        public string Article { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// URL медиафайлов.
        /// </summary>
        public List<string> MediaUrls { get; set; } = new();

        /// <summary>
        /// ID категорий, к которым относится товар.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new();
    }
}
