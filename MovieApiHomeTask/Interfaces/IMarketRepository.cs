using MarketPlaceApi.Dtos;
using MarketPlaceApi.Entities;

namespace MarketPlaceApi.Interfaces;

public interface IMarketRepository
{
    Task<ItemEntity?> AddItem(ItemDto item);
    Task<OrderEntity?> AddOrder(OrderDto order);
    Task<ItemDto?> GetItemById(int id);
    Task<OrderEntity?> UpdateOrderAsCompleted(int id);
    Task<List<UserOrderEntity>> GetUserOrders(int id);
    Task<DateTime> DeleteUnpaidOrdersAfterTimeLimit(DateTime time);
}