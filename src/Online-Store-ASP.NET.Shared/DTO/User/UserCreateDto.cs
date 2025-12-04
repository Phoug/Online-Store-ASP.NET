using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.User
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(64)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer";

        [Required]
        public DateTime BirthDate { get; set; }
    }
}
