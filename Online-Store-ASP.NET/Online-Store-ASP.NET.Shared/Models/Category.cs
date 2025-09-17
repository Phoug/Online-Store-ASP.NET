namespace Online_Store_ASP.NET.Shared.Models
{
    public class Category
    {
        // PK
        public int Id { get; set; }

        // Название категории
        public string Name { get; set; } = string.Empty;

        // Описание
        public string Description { get; set; } = string.Empty;
    }
}
