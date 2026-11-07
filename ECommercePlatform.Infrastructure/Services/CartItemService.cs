using AutoMapper;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.CartItem;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.CartItem;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public CartItemService(IMapper mapper,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
        }



        public async Task<List<GetCartItemDto>> AddItemToCartAsync(int productId, AddCartItemDto dto)
        {
            var userId = _currentUserService.UserId;

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null || product.IsDeleted)
            {
                throw new NotFoundException(nameof(Product), productId);
            }


            // Validate the dto
            var validator = new AddCartItemValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }



            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = _currentUserService.UserId
                };

                await _cartRepository.CreateAsync(cart);
            }



            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            // Check if the requested quantity exceeds the available stock
            int currentQuantityInCart = existingItem?.Quantity ?? 0;
            int requestedTotalQuantity = currentQuantityInCart + dto.Quantity;

            if (requestedTotalQuantity > product.StockQuantity)
            {
                throw new BadRequestException(
                    $"Requested quantity ({requestedTotalQuantity}) exceeds available stock ({product.StockQuantity}).");
            }

            // Check if the item already exists in the cart
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }

            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = dto.Quantity
                });
            }

            await _cartRepository.SaveChangesAsync();

            return _mapper.Map<List<GetCartItemDto>>(cart.CartItems);

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
