namespace Online_Store_ASP.NET.Shared.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public decimal DeliveryCost { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
