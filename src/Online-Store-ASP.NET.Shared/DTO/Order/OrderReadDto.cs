namespace Shared.DTO.Order
{
    /// <summary>
    /// DTO для отображения информации о заказе.
    /// </summary>
    public class OrderReadDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Итоговая стоимость заказа.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Текущий статус заказа.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Id пользователя, оформившего заказ.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Краткие данные о пользователе.
        /// </summary>
        public OrderUserDto User { get; set; } = new();

        /// <summary>
        /// Id доставки.
        /// </summary>
        public int DeliveryId { get; set; }

        /// <summary>
        /// Краткие сведения о доставке.
        /// </summary>
        public OrderDeliveryDto? Delivery { get; set; }
    }
}
