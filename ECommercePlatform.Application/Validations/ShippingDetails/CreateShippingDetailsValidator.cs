using ECommercePlatform.Application.DTOs.ShippingDetails;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.ShippingDetails
{
    public class CreateShippingDetailsValidator : AbstractValidator<CreateShippingDetailsDto>
    {
        public CreateShippingDetailsValidator()
        {
            RuleFor(sd => sd.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


            RuleFor(sd => sd.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


            RuleFor(sd => sd.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$").WithMessage("{PropertyName} must be a valid email address.")
                .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.");


            RuleFor(sd => sd.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^\+[1-9]\d{7,14}$")
                .WithMessage("{PropertyName} must be a valid international phone number in E.164 format. Example, +92XXXXXXXXXX")
                .MaximumLength(25).WithMessage("{PropertyName} cannot exceed 25 characters.");


            RuleFor(sd => sd.City)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


            RuleFor(sd => sd.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


            RuleFor(sd => sd.ShippingAddress)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


            RuleFor(sd => sd.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");
        }
    }
}
