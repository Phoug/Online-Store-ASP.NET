namespace Shared.DTO.Delivery
{
    /// <summary>
    /// Краткие сведения о заказе, к которому привязана доставка.
    /// </summary>
    public class DeliveryOrderDto
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
