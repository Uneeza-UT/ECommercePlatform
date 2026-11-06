using AutoMapper;
using ECommercePlatform.Application.Contracts.Email;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Order;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Models.Email;
using ECommercePlatform.Application.Validations.Order;
using ECommercePlatform.Domain;
using ECommercePlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly ICustomerCouponRepository _customerCouponRepository;
        private readonly IDeliverySettingsRepository _deliverySettingsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public OrderService(IMapper mapper,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICouponRepository couponRepository,
            ICustomerCouponRepository customerCouponRepository,
            IDeliverySettingsRepository deliverySettingsRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _customerCouponRepository = customerCouponRepository;
            _deliverySettingsRepository = deliverySettingsRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }



        public async Task<List<GetOrderDto>> GetAsync(PaginationDto dto)
        {
            string userId = _currentUserService.UserId;
            string userRole = _currentUserService.UserRole;

            switch (userRole)
            {
                case "Admin":
                    var allOrders = await _orderRepository.GetAllWithDetailsAsync(dto.PageNumber, dto.PageSize);
                    return _mapper.Map<List<GetOrderDto>>(allOrders);

                case "Customer":
                    var customerOrders = await _orderRepository.GetAllByUserIdWithDetailsAsync(userId, dto.PageNumber, dto.PageSize);
                    return _mapper.Map<List<GetOrderDto>>(customerOrders);

                default:
                    throw new ForbiddenException("You are not authorized to view orders.");
            }
        }



        public async Task<GetOrderDto> GetByIdAsync(int id)
        {
            string userId = _currentUserService.UserId;
            string userRole = _currentUserService.UserRole;

            var order = await _orderRepository.GetByIdWithDetailsAsync(id);

            if (order == null)
            {
                throw new NotFoundException(nameof(order), id);
            }

            switch (userRole)
            {
                case "Admin":
                    return _mapper.Map<GetOrderDto>(order);

                case "Customer":
                    if (order.UserId != userId)
                    {
                        throw new ForbiddenException("You can only view your own orders.");
                    }

                    return _mapper.Map<GetOrderDto>(order);

                default:
                    throw new ForbiddenException("You are not authorized to view orders.");
            }
        }


        public async Task<int> CreateAsync(CreateOrderDto dto)
        {
            string? userId = _currentUserService.UserId;
            Order order = new Order();
            List<OrderItem> orderItems = new List<OrderItem>();
            Cart? cart = null;
            Coupon? coupon = null;
            CustomerCoupon? customerCoupon = null;
            CouponUsage? couponUsage = null;
            decimal subTotal = 0;


            // Validate the dto
            var validator = new CreateOrderValidator(_currentUserService);
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid order entity. ", validationResult);
            }


            // Check if a coupon code is provided and retrieve the customer coupon

            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                coupon = await _couponRepository.GetByCodeAsync(dto.CouponCode);

                if (coupon == null)
                {
                    throw new NotFoundException(nameof(Coupon), dto.CouponCode);
                }

                customerCoupon = ValidateCoupon(coupon, dto.ShippingDetails.Email);

                couponUsage = new CouponUsage
                {
                    CouponId = coupon.Id
                };
            }        


            if (userId != null)
            {
                // Retrieve the user's cart for authenticated users and create order items based on the cart items

                cart = await _cartRepository.GetByUserIdAsync(userId);

                if (cart == null || !cart.CartItems.Any())
                {
                    throw new BadRequestException("Cannot place an order from an empty cart.");
                }

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

                    if (product == null || product.IsDeleted)
                    {
                        throw new NotFoundException(nameof(product), cartItem.ProductId);
                    }

                    ValidateStock(product, cartItem.Quantity);

                    orderItems.Add(new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = product.Price,
                    });

                    subTotal += product.Price * cartItem.Quantity;

                    // Update the stock quantity for each product in the order
                    product.StockQuantity -= cartItem.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                }      
            }

            else
            {
                foreach (var item in dto.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);

                    if (product == null || product.IsDeleted)
                    {
                        throw new NotFoundException(nameof(product), item.ProductId);
                    }

                    ValidateStock(product, item.Quantity);

                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                    });

                    subTotal += product.Price * item.Quantity;

                    // Update the stock quantity for each product in the order
                    product.StockQuantity -= item.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                }
            }


            decimal discountAmount = coupon != null
                    ? CalculateDiscountAmount(coupon, subTotal)
                    : 0;

            var (totalAmount, deliveryFee) = await CalculateTotalAmount(subTotal, discountAmount, dto.PaymentType);

            var payment = AddPaymentDetails(dto, totalAmount);

            dto.ShippingDetails.Email = dto.ShippingDetails.Email.Trim().ToLowerInvariant();

            ShippingDetails shippingDetails =
                _mapper.Map<ShippingDetails>(dto.ShippingDetails);


            order = new Order
            {
                UserId = userId,
                SubTotal = subTotal,
                DiscountAmount = discountAmount > 0 ? discountAmount : null,
                TotalAmount = totalAmount,
                DeliveryFee = deliveryFee,
                PaymentType = dto.PaymentType,
                ShippingDetails = shippingDetails,
                CouponUsage = couponUsage,
                Payment = payment,
                OrderItems = orderItems
            };


            var existingOrder = await _orderRepository.GetFirstOrderAsync(
                    userId,
                    userId == null ? dto.ShippingDetails.Email : null);


            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Create and save order
                await _orderRepository.CreateAsync(order);
                

                // Update coupon usage and used count if a coupon was applied
                if (customerCoupon != null)
                {   
                    customerCoupon.IsUsed = true;
                    coupon!.UsedCount++;
                    await _couponRepository.SaveChangesAsync();
                }


                // Save updated product stock
                await _productRepository.SaveChangesAsync();


                // Clear cart for authenticated customer
                if (cart != null)
                {
                    await _cartRepository.ClearCartAsync(userId);
                }


                if (existingOrder == null)
                {
                    // Assign FIRSTORDER coupon
                    await AssignFirstOrderCoupon(
                        userId,
                        userId == null ? dto.ShippingDetails.Email : null);
                }


                await _emailSender.SendEmailAsync(new EmailMessageData
                {
                    To = dto.ShippingDetails.Email,
                    Subject = $"Your Order Has Been Placed – Order #{order.Id}",
                    Body = $"Hello {dto.ShippingDetails.FirstName},\n\n" +
                        "Thank you for shopping with us!\n" +
                        "Your order has been successfully placed. Here are your order details:\n\n" +
                        $"Order ID: #{order.Id}\n" +
                        $"Order Date: {order.CreatedAt}\n" +
                        $"Total Amount: Rs. {order.TotalAmount}\n" +
                        $"Payment Method: {order.PaymentType}\n\n" +
                        "We will notify you when your order is confirmed and when it has been shipped.\n\n" +
                        "Thank you for choosing us!\n\n" +
                        "Best regards,\n" +
                        "The E-Commerce Team"
                });


                await _unitOfWork.CommitTransactionAsync();

                return order.Id;
            }

            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw new BadRequestException(
                    "Some order information has changed. Please refresh and try again.");
            }

            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }



        public async Task ChangeOrderStatusAsync(int id, ChangeOrderStatusDto dto)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(id);

            if (order == null)
            {
                throw new NotFoundException(nameof(order), id);
            }


            // Validate the dto
            var validator = new ChangeOrderStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid order status update. ", validationResult);
            }


            ValidateStatusTransition(order.OrderStatus, dto.OrderStatus);


            if (order.PaymentType == PaymentType.CashOnDelivery && dto.OrderStatus == OrderStatus.Delivered)
            {
                order.Payment?.PaidAt = DateTime.UtcNow;
            }

            order.OrderStatus = dto.OrderStatus;
            await _orderRepository.SaveChangesAsync();

            string confirmedOrderMessage = $"Your order #{order.Id} has been confirmed.\n\n" +
                $"Our team is now preparing your order for shipment.\n\n" +
                $"Order ID: #{order.Id}\n" +
                $"Order Total: Rs. {order.TotalAmount}\n\n" +
                $"We will notify you as soon as your order has been shipped.\n\n" +
                $"Thank you for shopping with us!\n\n";


            string shippedOrderMessage = $"Your order #{order.Id} has been shipped!.\n\n" +
               $"Your order is now on its way to the delivery address you provided.\n\n" +
               $"Order ID: #{order.Id}\n" +
               $"Order Total: Rs. {order.TotalAmount}\n" +
               $"Shipping Address: {order.ShippingDetails.ShippingAddress}\n\n" +
               $"We will notify you when your order has been delivered.\n\n" +
               $"Thank you for shopping with us!\n\n";


            string deliveredOrderMessage = $"Your order #{order.Id} has been successfully delivered!.\n\n" +
               $"We hope you enjoy your purchase. Thank you for shopping with us.\n\n" +
               $"Order ID: #{order.Id}\n" +
               $"Order Total: Rs. {order.TotalAmount}\n\n" +
               $"We appreciate your trust in us and look forward to serving you again.\n\n";


            string cancelledOrderMessage = $"We're sorry to inform you that your order #{order.Id} has been cancelled.\n\n" +
               $"Order ID: #{order.Id}\n" +
               $"Order Total: Rs. {order.TotalAmount}\n\n" +
               $"If you have already made an online payment, any applicable refund will be handled according to our refund policy.\n" +
               $"If you have questions or need assistance, please contact our customer support team.\n\n" +
               $"We apologize for any inconvenience.\n\n";


            string emailMessage = order.OrderStatus switch
            {
                OrderStatus.Confirmed => confirmedOrderMessage,
                OrderStatus.Shipped => shippedOrderMessage,
                OrderStatus.Delivered => deliveredOrderMessage,
                OrderStatus.Cancelled => cancelledOrderMessage,
                _ => throw new InvalidOperationException("Unsupported order status for email notification.")
            };


            await _emailSender.SendEmailAsync(new EmailMessageData
            {
                To = order.ShippingDetails.Email,
                Subject = $"Your Order Has Been {order.OrderStatus} – Order #{order.Id}",
                Body = $"Hello {order.ShippingDetails.FirstName},\n\n" +
                        $"{emailMessage}" +
                        "Best regards,\n" +
                        "The E-Commerce Team"
            });
        }



        private CustomerCoupon ValidateCoupon(Coupon coupon, string customerEmail)
        {
            CustomerCoupon? customerCoupon = null;
            var userId = _currentUserService.UserId;
            customerEmail = customerEmail.Trim().ToLowerInvariant();


            if (userId == null)
            {
                customerCoupon = coupon.CustomerCoupons
                    .FirstOrDefault(cc => 
                        cc.CustomerId == null && 
                        cc.GuestEmail== customerEmail && 
                        cc.CouponId == coupon.Id);
            }

            else
            {
                customerCoupon = coupon.CustomerCoupons
                    .FirstOrDefault(cc =>
                        cc.CustomerId == userId &&
                        cc.GuestEmail == null &&
                        cc.CouponId == coupon.Id);
            }
           
            var now = DateTime.UtcNow;

            if (customerCoupon == null)
            {
                throw new BadRequestException(
                    $"You can only use coupons assigned to you.");
            }


            if (customerCoupon.IsUsed)
            {
                throw new BadRequestException(
                    $"This coupon '{coupon.Code}' has already been used.");
            }


            if (customerCoupon.ExpiryDate <= now)
            {
                throw new BadRequestException($"This coupon '{customerCoupon.Coupon.Code}' has already expired.");
            }


            if (coupon.CouponStatus != CouponStatus.Active)
            {
                throw new BadRequestException($"This coupon '{customerCoupon.Coupon.Code}' is currently inactive.");
            }

            return customerCoupon;
        }


        private void ValidateStock(Product product, int quantity)
        {
            if (quantity > product.StockQuantity)
            {
                throw new BadRequestException(
                    $"Requested quantity for product '{product.Id}' ({quantity}) exceeds " +
                    $"available stock ({product.StockQuantity}).");
            }
        }


        private decimal CalculateDiscountAmount(Coupon coupon, decimal subtotal)
        {
            decimal discountAmount = 0;

            if (coupon.MinimumOrderAmount > subtotal)
            {
                throw new BadRequestException(
                    $"This coupon '{coupon.Code}' requires a minimum order amount of {coupon.MinimumOrderAmount}.");
            }

            if (coupon.CouponType == CouponType.Percentage)
            {
                discountAmount = subtotal * coupon.DiscountPercentage!.Value / 100;

                if (coupon.MaximumDiscount.HasValue)
                {
                    discountAmount = Math.Min(
                        discountAmount,
                        coupon.MaximumDiscount.Value);
                }
            }

            else if (coupon.CouponType == CouponType.FixedAmount)
            {
                discountAmount = coupon.DiscountAmount!.Value;
            }

            discountAmount = Math.Min(discountAmount, subtotal);

            return discountAmount;
        }



        private async Task<(decimal totalAmount, decimal deliveryFee)> CalculateTotalAmount
            (decimal subtotal, decimal discountAmount, PaymentType paymentType)
        {
            decimal deliveryFee = 0;
            decimal totalAmount = subtotal - discountAmount;

            // Retrieve delivery settings
            var deliverySettings = await _deliverySettingsRepository.GetByIdAsync(1);

            if (deliverySettings != null)
            {
                if (paymentType == PaymentType.CashOnDelivery)
                {
                    deliveryFee = deliverySettings.CashOnDeliveryFee;
                }

                else if (paymentType == PaymentType.CreditDebitCard || paymentType == PaymentType.BankTransfer)
                {
                    deliveryFee = deliverySettings.OnlinePaymentDeliveryFee;
                }
            }

            totalAmount += deliveryFee;

            return (totalAmount, deliveryFee);
        }


        private Payment AddPaymentDetails(CreateOrderDto dto, decimal totalAmount)
        {
            Payment payment = new Payment();

            // Add payment details based on the selected payment type

            if (dto.PaymentType == PaymentType.CreditDebitCard || dto.PaymentType == PaymentType.BankTransfer)
            {
                payment = new Payment
                {
                    Amount = totalAmount,
                    Status = PaymentStatus.Paid,
                    TransactionReference = $"SIM-{Guid.NewGuid():N}",
                    PaidAt = DateTime.UtcNow
                };
            }


            else if (dto.PaymentType == PaymentType.CashOnDelivery)
            {
                payment = new Payment
                {
                    Amount = totalAmount,
                    Status = PaymentStatus.Pending
                };
            }

            else
            {
                throw new BadRequestException("Invalid payment type.");
            }

            return payment;
        }


        private async Task AssignFirstOrderCoupon(string? userId, string? guestEmail)
        {
            var coupon = await _couponRepository.GetByIdAsync(2);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), 2);
            }


            int validityDays = coupon.ValidityDays ?? 30;

            var customerCoupon = new CustomerCoupon
            {
                CouponId = coupon.Id,
                CustomerId = userId,
                GuestEmail = guestEmail,
                ExpiryDate = DateTime.UtcNow.AddDays(validityDays),
                AssignedAt = DateTime.UtcNow
            };

            await _customerCouponRepository.CreateAsync(customerCoupon);
        }



        private void ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                throw new BadRequestException(
                    $"Order is already {currentStatus}.");
            }


            // Check if the order is already delivered or cancelled
            if (currentStatus is OrderStatus.Delivered or OrderStatus.Cancelled)
            {
                throw new BadRequestException($"Cannot change status of an order that is already {currentStatus}.");
            }


            if (currentStatus != OrderStatus.Confirmed && newStatus == OrderStatus.Shipped)
            {
                throw new BadRequestException("Order can only be marked as Shipped after confirmation.");
            }


            if (currentStatus != OrderStatus.Shipped && newStatus == OrderStatus.Delivered)
            {
                throw new BadRequestException("Order can only be marked as Delivered after being shipped.");
            }


            if (currentStatus == OrderStatus.Shipped && newStatus == OrderStatus.Cancelled)
            {
                throw new BadRequestException("Shipped orders cannot be cancelled.");
            }
        }
    }
}
