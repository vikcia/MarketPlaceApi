using MarketPlaceApi.Dtos;
using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;

namespace MarketPlaceApi.Services;

public class AuthenticateService
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;

    public AuthenticateService(IUserRepository userRepository, IJWTService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> CheckLoginData(LoginDto loginDto)
    {
        UserEntity userEntity = await _userRepository.Get(loginDto.UserName)
                                ?? throw new UnauthorizedAccessException();

        if (loginDto.Password != userEntity.Password)
            throw new UnauthorizedAccessException();

        return await _jwtService.GenerateToken(loginDto);
    }
}
