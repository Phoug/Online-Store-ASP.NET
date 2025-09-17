namespace Online_Store_ASP.NET.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
