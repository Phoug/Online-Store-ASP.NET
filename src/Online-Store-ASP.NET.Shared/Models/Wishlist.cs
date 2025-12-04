using Online_Store_ASP_NET.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Wishlist
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [JsonIgnore] // Добавьте этот атрибут
    public virtual User? User { get; set; }

    public virtual ICollection<WishlistItem>? WishlistItems { get; set; }
}
