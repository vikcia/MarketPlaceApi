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
            .NotEmpty().WithMessage("User ID is required.");
    }
}
