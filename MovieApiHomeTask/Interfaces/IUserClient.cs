using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Interfaces;

public interface IUserClient
{
    Task<UserDto> GetUser(int userId);
}