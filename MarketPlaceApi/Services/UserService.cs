using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;

namespace MarketPlaceApi.Services;

public enum UserResponsibilities
{
    Administrator,
    Seller,
    Client
}

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Add(User user)
    {
        UserEntity? userEntity = await _userRepository.Get(user.Name);

        if (userEntity is not null)
            throw new Exception($"User {user.Name} allready exist");

        userEntity = new()
        {
            Name = user.Name,
            Password = user.Password,
            Role = user.Role
        };

        return await _userRepository.Add(userEntity);
    }
}