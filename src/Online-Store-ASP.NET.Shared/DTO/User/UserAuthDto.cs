using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP_NET.Shared.DTO.User
{
    public class UserAuthDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
