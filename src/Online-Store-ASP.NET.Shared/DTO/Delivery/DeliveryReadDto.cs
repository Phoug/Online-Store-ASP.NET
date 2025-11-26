namespace Shared.DTO.Delivery
{
    /// <summary>
    /// DTO для отображения данных о доставке.
    /// </summary>
    public class DeliveryReadDto
    {
        public int Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public string DeliveryMethod { get; set; } = string.Empty;

        public decimal DeliveryCost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int OrderId { get; set; }
        public DeliveryOrderDto Order { get; set; } = new();

        public int? UserId { get; set; }
        public DeliveryUserDto? User { get; set; }
    }
}
