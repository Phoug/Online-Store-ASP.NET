using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Order
{
    /// <summary>
    /// DTO для создания заказа.
    /// </summary>
    public class OrderCreateDto
    {
        /// <summary>
        /// Id пользователя, создающего заказ.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Id доставки (опционально).
        /// </summary>
        public int? DeliveryId { get; set; }

        /// <summary>
        /// Статус при создании (опционально).
        /// Если не указан — будет назначен в сервисе.
        /// </summary>
        public string? Status { get; set; }
    }
}
