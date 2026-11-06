using AutoMapper;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.Coupon;
using ECommercePlatform.Domain;
using ECommercePlatform.Domain.Enums;
using ECommercePlatform.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CouponService : ICouponService
    {
        private readonly IMapper _mapper;
        private readonly ICouponRepository _couponRepository;
        private readonly ICustomerCouponRepository _customerCouponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CouponService(IMapper mapper,
            ICouponRepository couponRepository,
            ICustomerCouponRepository customerCouponRepository,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _couponRepository = couponRepository;
            _customerCouponRepository = customerCouponRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<GetCouponDto>> GetAsync(PaginationDto dto)
        {
            var coupons = await _couponRepository.GetAllWithDetailsAsync(dto.PageNumber, dto.PageSize);
            return _mapper.Map<List<GetCouponDto>>(coupons);
        }



        public async Task<GetCouponDto> GetByIdAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdWithDetailsAsync(id);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), id);
            }

            return _mapper.Map<GetCouponDto>(coupon);
        }



        public async Task<int> CreateAsync(CreateCouponDto dto)
        {
            // Validate the dto
            var validator = new CreateCouponValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Coupon code is unique
            var isCodeUnique = await _couponRepository.IsCodeUnique(dto.Code);

            if (!isCodeUnique)
            {
                throw new ConflictException("Coupon Code already exists.");
            }


            var coupon = _mapper.Map<Coupon>(dto);
            await _couponRepository.CreateAsync(coupon);
            return coupon.Id;
        }



        public async Task UpdateAsync(int id, UpdateCouponDto dto)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), id);
            }


            // Validate the dto
            var validator = new UpdateCouponValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Coupon code is unique
            var isCodeUnique = await _couponRepository.IsCodeUnique(dto.Code, id);

            if (!isCodeUnique)
            {
                throw new ConflictException("Coupon Code already exists.");
            }


            _mapper.Map(dto, coupon);
            coupon.UpdatedAt = DateTime.UtcNow;

            await _couponRepository.SaveChangesAsync();
        }


        public async Task ChangeStatusAsync(int id, ChangeCouponStatusDto dto)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), id);
            }

            // Validate the dto
            var validator = new ChangeCouponStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }


            if (coupon.CouponStatus == CouponStatus.Expired)
            {
                throw new BadRequestException("Cannot change status of an Expired coupon.");
            }


            if (dto.CouponStatus == CouponStatus.Expired)
            {
                throw new BadRequestException(
                    "Coupon status cannot be manually changed to Expired.");
            }


            coupon.CouponStatus = dto.CouponStatus;
            coupon.UpdatedAt = DateTime.UtcNow;

            await _couponRepository.SaveChangesAsync();
        }



        public async Task AssignToCustomerAsync(int id, AssignCouponDto dto)
        {
            var coupon = await _couponRepository.GetByIdWithDetailsAsync(id);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), id);
            }


            // Validate the dto
            var validator = new AssignCouponValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }


            string? guestEmail = string.IsNullOrWhiteSpace(dto.GuestEmail)
                ? null
                : dto.GuestEmail.Trim().ToLowerInvariant();


            if (!string.IsNullOrWhiteSpace(dto.CustomerId))
            {
                var user = await _userManager.FindByIdAsync(dto.CustomerId);

                if (user == null)
                {
                    throw new NotFoundException(nameof(ApplicationUser), dto.CustomerId);
                }


                if (coupon.CustomerCoupons.Any(cc => cc.CustomerId == dto.CustomerId))
                {
                    throw new ConflictException("Coupon is already assigned to this customer.");
                }           
            }


            else if (!string.IsNullOrWhiteSpace(guestEmail))
            {
                if (coupon.CustomerCoupons.Any(cc => cc.GuestEmail == guestEmail))
                {
                    throw new ConflictException("Coupon is already assigned to this guest customer.");
                }


                if (coupon.AssignmentType == CouponAssignmentType.CustomerSpecific)
                {
                    throw new BadRequestException(
                        "Customer specific coupons can only be assigned to registered customers.");
                }
            }



            if (coupon.CouponStatus != CouponStatus.Active)
            {
                throw new BadRequestException("Only active coupons can be assigned to customers.");
            }


            var now = DateTime.UtcNow;

            if (coupon.StartsAt.HasValue && coupon.StartsAt.Value > now)
            {
                throw new BadRequestException("Coupon is not available yet.");
            }


            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value <= now)
            {
                throw new BadRequestException("Coupon has already expired.");
            }


            if (coupon.AssignmentLimit.HasValue && coupon.AssignedCount >= coupon.AssignmentLimit.Value)
            {
                throw new BadRequestException("Coupon has reached its assignment limit.");
            }


            var assignedAt = now;

            int validityDays = coupon.ValidityDays ?? 30;

            var expiryDate = assignedAt.AddDays(validityDays);

            if (coupon.ExpiryDate.HasValue &&
                coupon.ExpiryDate.Value < expiryDate)
            {
                expiryDate = coupon.ExpiryDate.Value;
            }


            var customerCoupon = new CustomerCoupon
            {
                CouponId = id,
                CustomerId = dto.CustomerId,
                GuestEmail = guestEmail,
                ExpiryDate = expiryDate,
                AssignedAt = assignedAt
            };


            try
            {
                await _customerCouponRepository.CreateAsync(customerCoupon);

                coupon.AssignedCount++;
                await _couponRepository.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }

            catch (DbUpdateException ex) when (
                ex.InnerException is SqlException sqlEx &&
                (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw new ConflictException(
                    "Coupon is already assigned to this customer.");
            }

            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }



        public async Task DeleteAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null || coupon.IsDeleted)
            {
                throw new NotFoundException(nameof(Coupon), id);
            }

            coupon.IsDeleted = true;

            await _couponRepository.SaveChangesAsync();
        }
    }
}
