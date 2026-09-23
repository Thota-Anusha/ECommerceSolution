namespace ECommerce.Models
{
    public class CartResponse
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
    }
}