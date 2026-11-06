using ECommercePlatform.Application.DTOs.CartItem;

namespace ECommercePlatform.Application.DTOs.Cart
{
    public class GetCartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<GetCartItemDto>? CartItems { get; set; } = new List<GetCartItemDto>();
    }
}
