using System.Collections.Generic;

namespace Online_Store_ASP_NET.Shared.DTO.Product
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<string> MediaUrls { get; set; } = new List<string>();
        public List<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
    }
}