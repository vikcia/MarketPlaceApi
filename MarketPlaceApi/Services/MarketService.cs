using MarketPlaceApi.CustomException;
using MarketPlaceApi.Dtos;
using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Validators;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Services;

public enum Status
{
    InProgress,
    Completed,
    Deleted
}

public class MarketService : IMarketService
{
    private readonly IMarketRepository _marketRepository;
    private readonly IItemDtoValidator _itemDtoValidator;
    private readonly IOrderDtoValidator _orderDtoValidator;
    private readonly IUserClient _userClient;

    public MarketService(IMarketRepository marketRepository, IItemDtoValidator itemDtoValidator, IOrderDtoValidator orderDtoValidator, IUserClient userClient)
    {
        _marketRepository = marketRepository;
        _itemDtoValidator = itemDtoValidator;
        _orderDtoValidator = orderDtoValidator;
        _userClient = userClient;
    }

    public async Task<ItemDto> AddItem(ItemDto item)
    {
        var result = _itemDtoValidator.Validate(item);

        if (!result.IsValid)
        {
            var validationErrors = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            throw new BadRequestException(validationErrors);
        }

        ItemEntity itemEntity = await _marketRepository.AddItem(item) ?? throw new NotFoundException("No data");

        return new()
        {
            Name = itemEntity.Name,
            Price = itemEntity.Price
        };
    }

    public async Task<OrderDto> AddOrder(OrderDto order)
    {
        _ = await _marketRepository.GetItemById(order.ItemId) ?? throw new ItemNotFoundException();

        var result = _orderDtoValidator.Validate(order);

        if (!result.IsValid)
        {
            var validationErrors = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            throw new BadRequestException(validationErrors);
        }

        OrderEntity orderEntity = await _marketRepository.AddOrder(order) ?? throw new NotFoundException("No data");

        return new()
        {
            ItemId = orderEntity.Id,
            UserId = orderEntity.UserId
        };
    }

    public async Task UpdateOrderAsCompleted(int id)
    {
        _ = await _marketRepository.UpdateOrderAsCompleted(id) ?? throw new NotFoundException("No orders by this ID");
    }

    public async Task<UserOrdersDto> GetUserOrders(int id)
    {

        UserDto user = await GetUser(id);

        List<UserOrderEntity> userOrderEntity = await _marketRepository.GetUserOrders(id) ?? throw new NotFoundException("User doesn't exist");

        return new()
        {
            User = user,
            Items = userOrderEntity
        };
    }

    public async Task<UserDto> GetUser(int id)
    {
        UserDto result = await _userClient.GetUser(id);

        if (result is null)
        {
            throw new NotFoundException("User not found");
        }
        return result;
    }

    public async Task<int> DeleteUnpaidOrdersAfterTimeLimit(DateTime time)
    {
        return await _marketRepository.DeleteUnpaidOrdersAfterTimeLimit(time);
    }
}