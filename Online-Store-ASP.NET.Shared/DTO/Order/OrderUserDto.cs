namespace Shared.DTO.Order
{
    /// <summary>
    /// Краткая информация о пользователе, оформившем заказ.
    /// </summary>
    public class OrderUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
