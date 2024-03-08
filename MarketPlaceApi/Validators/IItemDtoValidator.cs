using FluentValidation.Results;
using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Validators;

public interface IItemDtoValidator
{
    ValidationResult Validate(ItemDto item);
}
