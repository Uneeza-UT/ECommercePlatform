using ECommercePlatform.Application.DTOs.Product;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");


            RuleFor(p => p.ImageUrl)
                .NotEmpty().WithMessage("{PropertyName} is required.");


            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");


            RuleFor(p => p.ProductsSold)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");


            RuleFor(p => p.DiscountPercentage)
                .GreaterThanOrEqualTo(0)
                .When(p => p.DiscountPercentage.HasValue)
                .WithMessage("{PropertyName} cannot be negative.");


            RuleFor(p => p.DiscountPercentage)
                .LessThanOrEqualTo(100)
                .When(p => p.DiscountPercentage.HasValue)
                .WithMessage("{PropertyName} cannot exceed 100%.");   
        }
    }
}
