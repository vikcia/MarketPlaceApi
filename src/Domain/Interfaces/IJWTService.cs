using MarketPlaceApi.Dtos;

namespace MarketPlaceApi.Interfaces;

public interface IJWTService
{
    Task<string> GenerateToken(LoginDto loginDto);
}