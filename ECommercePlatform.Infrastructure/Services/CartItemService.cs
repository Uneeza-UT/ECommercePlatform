using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;

        public CartItemService(ICartRepository cartRepository,
            ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _currentUserService = currentUserService;
        }


        public async Task UpdateQuantityAsync(int productId, bool increment)
        {
            var (cart, cartItem) = await ValidateCartAndCartItemAsync(productId);

            if (increment)
            {
                // Check if the requested quantity exceeds the available stock
                int requestedQuantity = cartItem.Quantity + 1;

                if (requestedQuantity > cartItem.Product.StockQuantity)
                {
                    throw new BadRequestException(
                        $"Requested quantity ({requestedQuantity}) exceeds available " +
                        $"stock ({cartItem.Product.StockQuantity}).");
                }

                cartItem.Quantity = requestedQuantity;
            }

            else
            {
                cartItem.Quantity--;

                if (cartItem.Quantity <= 0)
                {
                    cart.CartItems.Remove(cartItem);
                }
            }

            await _cartRepository.SaveChangesAsync();
        }


        public async Task RemoveItemFromCartAsync(int productId)
        {
            var (cart, cartItem) = await ValidateCartAndCartItemAsync(productId);

            cart.CartItems.Remove(cartItem);
            await _cartRepository.SaveChangesAsync();
        }


        private async Task<(Cart Cart, CartItem CartItem)> ValidateCartAndCartItemAsync(int productId)
        {     
            var userId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found.");
            }


            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new NotFoundException("Cart item not found.");
            }

            return (cart, cartItem);
        }
    }
}
