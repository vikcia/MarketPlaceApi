using FluentValidation;
using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Validators;

public class ItemDtoValidator : AbstractValidator<ItemDto>, IItemDtoValidator
{
    public ItemDtoValidator()
    {
        RuleFor(value => value.Name)
            .NotEmpty().WithMessage("Name is required.");
        RuleFor(value => value.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}