namespace Shared.DTO.Cart
{
    /// <summary>
    /// DTO для обновления корзины.
    /// Обычно используется для смены владельца корзины.
    /// </summary>
    public class CartUpdateDto
    {
        /// <summary>
        /// Новый идентификатор пользователя (опционально).
        /// </summary>
        public int? UserId { get; set; }
    }
}
