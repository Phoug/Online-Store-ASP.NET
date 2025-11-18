using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Delivery
{
    /// <summary>
    /// DTO для создания доставки.
    /// </summary>
    public class DeliveryCreateDto
    {
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string DeliveryMethod { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal DeliveryCost { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int OrderId { get; set; }

        public int? UserId { get; set; }
    }
}
