using ECommerce.Models;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);

        Task<Cart?> GetCartByIdAsync(int cartId);

        Task<List<CartItem>> GetCartItemsAsync(int cartId);

        Task AddCartAsync(Cart cart);

        Task<CartItem?> GetCartItemAsync(
            int cartId,
            int productId);

        Task AddCartItemAsync(CartItem cartItem);

        Task UpdateCartItemAsync(CartItem cartItem);

        Task DeleteCartItemAsync(CartItem cartItem);

        Task SaveChangesAsync();
    }
}