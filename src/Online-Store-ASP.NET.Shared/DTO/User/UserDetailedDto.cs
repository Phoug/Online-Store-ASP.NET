using Shared.DTO.Cart;
using Shared.DTO.Wishlist;
using Shared.DTO.Review;
using Shared.DTO.Order;
using Shared.DTO.Delivery;

namespace Online_Store_ASP_NET.Shared.DTO.User
{
    /// <summary>
    /// Полное DTO пользователя.
    /// Содержит профиль, корзину, избранное, заказы, отзывы и доставки.
    /// </summary>
    public class UserDetailedDto
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Полное имя пользователя.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Телефон пользователя.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Роль пользователя: Admin, Manager, Customer, Guest.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Дата регистрации пользователя.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Дата рождения пользователя.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Подробная информация о корзине.
        /// </summary>
        public CartReadDto? Cart { get; set; }

        /// <summary>
        /// Подробная информация об избранном.
        /// </summary>
        public WishlistDetailedDto? Wishlist { get; set; }

        /// <summary>
        /// Короткая информация об отзывах пользователя.
        /// </summary>
        public List<ReviewAuthorDto> Reviews { get; set; } = new();

        /// <summary>
        /// Короткая информация о заказах пользователя.
        /// </summary>
        public List<OrderReadDto> Orders { get; set; } = new();

        /// <summary>
        /// Короткая информация о доставках пользователя.
        /// </summary>
        public List<DeliveryUserDto> Deliveries { get; set; } = new();
    }
}
