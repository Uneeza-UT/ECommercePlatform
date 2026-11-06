using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class ShippingDetails : BaseEntity
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; } 
        public string ShippingAddress { get; set; }   
        public string? PostalCode { get; set; }
    }
}
