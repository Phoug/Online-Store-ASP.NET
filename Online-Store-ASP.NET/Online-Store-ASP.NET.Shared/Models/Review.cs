namespace Online_Store_ASP.NET.Shared.Models
{
    public class Review
    {
        // PK
        public int Id { get; set; }

        // Оценка, оставленная с отзывом
        public int Rating { get; set; }

        // Текст отзыва
        public string Comment { get; set; } = string.Empty;
    }
}
