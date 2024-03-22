using MarketPlaceApi.Entities;

namespace MarketPlaceApi.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Add(UserEntity user);
        Task<UserEntity?> Get(Guid id);
        Task<UserEntity?> Get(string name);
    }
}