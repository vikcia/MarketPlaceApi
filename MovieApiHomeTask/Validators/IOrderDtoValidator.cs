using FluentValidation.Results;
using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Validators;

public interface IOrderDtoValidator
{
    ValidationResult Validate(OrderDto order);
}
