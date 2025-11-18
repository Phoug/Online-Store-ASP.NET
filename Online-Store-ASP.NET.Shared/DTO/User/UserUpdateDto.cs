using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.User
{
    public class UserUpdateDto
    {
        [MaxLength(64)]
        public string? Name { get; set; }

        [MaxLength(64)]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Role { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
