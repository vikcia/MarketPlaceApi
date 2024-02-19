using MarketPlaceApi.Entities;

namespace MarketPlaceApi.Dtos;

public class UserOrdersDto
{
    public UserDto User { get; set; } = new UserDto();
    public List<UserOrderEntity> Items { get; set; } = new List<UserOrderEntity>();
}