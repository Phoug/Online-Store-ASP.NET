namespace Shared.DTO.Review
{
    /// <summary>
    /// Краткое описание автора отзыва.
    /// </summary>
    public class ReviewAuthorDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}
