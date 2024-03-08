using FluentValidation;
using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Validators;

public class OrderDtoValidator : AbstractValidator<OrderDto>, IOrderDtoValidator
{
    public OrderDtoValidator()
    {
        RuleFor(value => value.ItemId)
            .NotEmpty().WithMessage("Item ID is required.");
        RuleFor(value => value.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .InclusiveBetween(1, 10).WithMessage("User ID must be between 1 and 10.");
    }
}
