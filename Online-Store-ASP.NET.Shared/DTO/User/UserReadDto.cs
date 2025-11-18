namespace Shared.DTO.User
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public int CartId { get; set; }
        public int WishlistId { get; set; }
        public int ReviewsCount { get; set; }
        public int OrdersCount { get; set; }
        public int DeliveriesCount { get; set; }
    }
}
