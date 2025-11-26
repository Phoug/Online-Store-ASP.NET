namespace Shared.DTO.Delivery
{
    /// <summary>
    /// Упрощённый DTO пользователя, получателя доставки.
    /// </summary>
    public class DeliveryUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}
