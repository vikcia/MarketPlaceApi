using MarketPlaceApi.Dtos;
using System.Threading.Tasks;

namespace MarketPlaceApi.Interfaces;

public interface IMarketService
{
    Task<ItemDto> AddItem(ItemDto item);
    Task<OrderDto> AddOrder(OrderDto order);
    Task UpdateOrderAsCompleted(int id);
    Task<UserDto> GetUser(int id);
    Task<UserOrdersDto> GetUserOrders(int id);
    Task<DateTime> DeleteUnpaidOrdersAfterTimeLimit(DateTime item);
}