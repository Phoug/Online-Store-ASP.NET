namespace Shared.DTO.CartItem
{
    /// <summary>
    /// DTO для создания нового элемента корзины.
    /// Используется, когда пользователь добавляет товар в корзину.
    /// </summary>
    public class CartItemCreateDto
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Идентификатор товара, который добавляется.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество добавляемого товара.
        /// </summary>
        public int Quantity { get; set; } = 1;
    }
}
