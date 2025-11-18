namespace Shared.DTO.CartItem
{
    /// <summary>
    /// DTO для обновления элемента корзины (например, изменение количества).
    /// </summary>
    public class CartItemUpdateDto
    {
        /// <summary>
        /// Новое количество товара.
        /// </summary>
        public int Quantity { get; set; }
    }
}
