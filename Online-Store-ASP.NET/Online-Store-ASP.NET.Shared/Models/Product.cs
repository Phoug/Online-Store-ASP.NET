namespace Online_Store_ASP.NET.Shared.Models
{
    public class Product
    {
        // PK
        public int Id { get; set; }

        // Название товара
        public string Name { get; set; } = string.Empty;

        // Описание товара
        public string Description { get; set; } = string.Empty;

        // Цена товара
        public decimal Price { get; set; }

        // Ссылки на фото/видео товара
        public string ImageUrl { get; set; } = string.Empty;

        // Количество товаров в наличии
        public int StockQuantity { get; set; }
    }
}
