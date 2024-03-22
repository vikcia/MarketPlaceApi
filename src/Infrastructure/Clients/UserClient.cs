using MarketPlaceApi.Dtos;
using MarketPlaceApi.Interfaces;

namespace MarketPlaceApi.Clients;

public class UserClient : IUserClient
{
    HttpClient _client;

    public UserClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<UserDto> GetUser(int userId)
    {
        var response = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");

        return await response.Content.ReadAsAsync<UserDto>(); ;
    }
}