namespace Shared.DTO.Order
{
    /// <summary>
    /// Краткие данные о доставке, связанные с заказом.
    /// </summary>
    public class OrderDeliveryDto
    {
        public int Id { get; set; }

        public string DeliveryMethod { get; set; } = string.Empty;

        public decimal DeliveryCost { get; set; }

        public string Address { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
