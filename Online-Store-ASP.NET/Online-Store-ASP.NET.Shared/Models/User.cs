namespace Online_Store_ASP.NET.Shared.Models
{
    public class User
    {
        // PK
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
