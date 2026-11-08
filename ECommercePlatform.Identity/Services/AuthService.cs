using ECommercePlatform.Application.Contracts.Email;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Application.DTOs.Identity;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Models.Email;
using ECommercePlatform.Application.Models.Identity;
using ECommercePlatform.Application.Validations.Identity;
using ECommercePlatform.Domain;
using ECommercePlatform.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommercePlatform.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartRepository _cartRepository;
        private readonly ICouponService _couponService;
        private readonly IEmailSender _emailSender;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            ICurrentUserService currentUserService,
            ICartRepository cartRepository,
            ICouponService couponService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _currentUserService = currentUserService;
            _cartRepository = cartRepository;
            _couponService = couponService;
            _emailSender = emailSender;
        }


        public async Task<AuthResponse> Login(AuthRequest request)
        {
            // Validate the request.
            var validator = new AuthRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid credentials. ", validationResult);

            }

            // Find the user by email.
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            }


            // Verify the user's password.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                throw new NotFoundException($"Credentials for {request.Email} aren't valid", request.Email);
            }


            // Generate a JWT token.
            JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);


            // Create the authentication response.
            var response = new AuthResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            return response;
        }



        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            // Retrieve the user's claims and roles.
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);


            // Convert the user's roles into role claims.
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();


            // Create the claims to include in the JWT token.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);


            // Create the security key and signing credentials.
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            // Create the JWT token with issuer, audience, claims, and expiration.
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }



        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            // Validate the request.
            var validator = new RegistrationRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid request. ", validationResult);
            }


            // Create a new user.
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }


            // Assign the selecetd role to the new user.
            await _userManager.AddToRoleAsync(user, "Customer");


            var cart = new Cart
            {
                UserId = user.Id
            };

            // Create a cart for the new User
            await _cartRepository.CreateAsync(cart);


            // Assign the "WELCOME2026" coupon to the new user
            var assignCouponToCustomerDto = new AssignCouponDto
            {
                CustomerId = user.Id
            };

            await _couponService.AssignToCustomerAsync(1, assignCouponToCustomerDto);


            // Notify the user about successful registration through email.
            await _emailSender.SendEmailAsync(new EmailMessageData
            {
                To = user.Email,
                Subject = "Welcome to Our E-Commerce Store!",
                Body = $"Hello {user.FirstName},\n\n" +
                       "Your account has been successfully created.\n\n" +
                       "You can now log in, explore our products, and start shopping.\n\n" +
                       "Thank you for joining us!\n\n" +
                       "Best regards,\n" +
                       "The E-Commerce Team"
            });


            return new RegistrationResponse
            {
                UserId = user.Id
            };
        }




        public async Task ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), _currentUserService.UserId);
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);


            if (!result.Succeeded)
            {
                string errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }

            await _emailSender.SendEmailAsync(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Your Password was Changed",
                Body = $"Hello {user.FirstName},\n\n" +
                        "Your password for our E-Commerce Platform has been changed.\n\n" +
                        "If you made this change, no further action is required.\n\n" +
                        "If you did not change your password, please contact our support team immediately.\n\n" +
                        "Thank you,\n\n" +
                        "Best regards,\n" +
                        "The E-Commerce Team"
            });
        }



        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            await _emailSender.SendEmailAsync(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Reset Your Password",
                Body = $"Hello {user.FirstName},\n\n" +
                        "We received a request to reset your password.\n\n" +
                        $"Reset your password using the following token:\n{token}\n\n" +
                        "If you did not request a password reset, you can safely ignore this email.\n\n" +
                        "Thank you,\n\n" +
                        "Best regards,\n" +
                        "The E-Commerce Team"
            });
        }




        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }


            await _emailSender.SendEmailAsync(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Your Password was Reset",
                Body = $"Hello {user.FirstName},\n\n" +
                        "Your password for our E-Commerce Platform has been changed.\n\n" +
                        "If you made this change, no further action is required.\n\n" +
                        "If you did not change your password, please contact our support team immediately.\n\n" +
                        "Thank you,\n\n" +
                        "Best regards,\n" +
                        "The E-Commerce Team"
            });
        }
    }
}
