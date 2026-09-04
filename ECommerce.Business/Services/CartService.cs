using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;

namespace ECommerce.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            return await _cartRepository
                .GetCartByUserIdAsync(userId);
        }

        public async Task<Cart> AddToCartAsync(
            int userId,
            int productId,
            int quantity)
        {
            var cart =
                await _cartRepository
                    .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _cartRepository.AddCartAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var existingItem =
                await _cartRepository.GetCartItemAsync(
                    cart.CartId,
                    productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;

                await _cartRepository
                    .UpdateCartItemAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _cartRepository
                    .AddCartItemAsync(cartItem);
            }

            await _cartRepository.SaveChangesAsync();

            return cart;
        }

        public async Task<bool> UpdateCartItemAsync(
            int userId,
            int productId,
            int quantity)
        {
            var cart =
                await _cartRepository
                    .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }

            var item =
                await _cartRepository.GetCartItemAsync(
                    cart.CartId,
                    productId);

            if (item == null)
            {
                return false;
            }

            item.Quantity = quantity;

            await _cartRepository
                .UpdateCartItemAsync(item);

            await _cartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromCartAsync(
            int userId,
            int productId)
        {
            var cart =
                await _cartRepository
                    .GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }

            var item =
                await _cartRepository.GetCartItemAsync(
                    cart.CartId,
                    productId);

            if (item == null)
            {
                return false;
            }

            await _cartRepository
                .DeleteCartItemAsync(item);

            await _cartRepository.SaveChangesAsync();

            return true;
        }
    }
}