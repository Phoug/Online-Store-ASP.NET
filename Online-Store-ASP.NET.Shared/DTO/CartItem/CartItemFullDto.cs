namespace Shared.DTO.CartItem
{
    /// <summary>
    /// Полный DTO для элемента корзины.
    /// Подходит для сервисов и административных API.
    /// </summary>
    public class CartItemFullDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена за единицу товара.
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Итоговая стоимость.
        /// </summary>
        public decimal Total => Quantity * ProductPrice;
    }
}
