namespace Shared.DTO.Order
{
    /// <summary>
    /// DTO для обновления заказа.
    /// </summary>
    public class OrderUpdateDto
    {
        /// <summary>
        /// Новый статус (опционально).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Новый Id доставки (опционально).
        /// </summary>
        public int? DeliveryId { get; set; }
    }
}
