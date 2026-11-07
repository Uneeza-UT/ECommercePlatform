using AutoMapper;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Cart;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;

        public CartService(IMapper mapper,
            ICartRepository cartRepository,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
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

    }
}
