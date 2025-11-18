using Shared.DTO.CartItem;

namespace Shared.DTO.Cart
{
    /// <summary>
    /// DTO для отображения корзины пользователю.
    /// Включает общую стоимость и список элементов корзины.
    /// </summary>
    public class CartReadDto
    {
        /// <summary>
        /// Уникальный идентификатор корзины.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит корзина.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Общая стоимость корзины.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Список элементов корзины.
        /// </summary>
        public List<CartItemReadDto> Items { get; set; } = new();
    }
}
