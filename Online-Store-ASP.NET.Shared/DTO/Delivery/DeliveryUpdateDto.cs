using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Delivery
{
    /// <summary>
    /// DTO для обновления доставки.
    /// </summary>
    public class DeliveryUpdateDto
    {
        public string? Address { get; set; }

        public string? DeliveryMethod { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DeliveryCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? UserId { get; set; }
    }
}
