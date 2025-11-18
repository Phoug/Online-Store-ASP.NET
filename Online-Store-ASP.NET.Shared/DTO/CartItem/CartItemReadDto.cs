namespace Shared.DTO.CartItem
{
    /// <summary>
    /// DTO для чтения информации об элементе корзины.
    /// Содержит данные товара, количество и итоговую стоимость.
    /// </summary>
    public class CartItemReadDto
    {
        /// <summary>
        /// Уникальный идентификатор элемента корзины.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Цена одного товара.
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Количество данного товара в корзине.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Общая стоимость (Quantity * ProductPrice).
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
