using ECommerce.Models;

namespace ECommerce.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> GetCartAsync(int userId);

        Task<Cart> AddToCartAsync(
            int userId,
            int productId,
            int quantity);

        Task<bool> UpdateCartItemAsync(
            int userId,
            int productId,
            int quantity);

        Task<bool> RemoveFromCartAsync(
            int userId,
            int productId);
    }
}