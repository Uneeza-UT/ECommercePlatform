using AutoMapper;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Cart;
using ECommercePlatform.Application.DTOs.CartItem;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.CartItem;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public CartService(IMapper mapper,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
        }


        public async Task<GetCartDto> GetAsync()
        {
            string userId = _currentUserService.UserId;
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            return _mapper.Map<GetCartDto>(cart);
        }



        public async Task CreateAsync()
        {
            var existingCart = await _cartRepository.GetByUserIdAsync(_currentUserService.UserId);

            if (existingCart != null)
            {
                throw new ConflictException("User already has a cart.");
            }

            var cart = new Cart
            {
                UserId = _currentUserService.UserId
            };

            await _cartRepository.CreateAsync(cart);
        }


        public async Task AddItemToCartAsync(AddCartItemDto dto)
        {
            var userId = _currentUserService.UserId;

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
                await CreateAsync();
            }



            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null || product.IsDeleted)
            {
                throw new NotFoundException(nameof(Product), dto.ProductId);
            }



            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

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
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _cartRepository.SaveChangesAsync();
         
        }
    }
}
