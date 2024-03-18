using Dapper;
using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Services;
using System.Data;

namespace MarketPlaceApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<UserEntity?> Get(Guid id)
    {
        string sql = @"SELECT id FROM users
                            WHERE id=@Id";

        var queryArguments = new { Id = id };

        return await _dbConnection.QuerySingleOrDefaultAsync<UserEntity>(sql, queryArguments);
    }

    public async Task<UserEntity?> Get(string name)
    {
        string sql = @"SELECT 
                            name, 
                            password, 
                            role 
                        FROM users
                        WHERE name=@Name";

        var queryArguments = new { Name = name };

        return await _dbConnection.QuerySingleOrDefaultAsync<UserEntity>(sql, queryArguments);
    }

    public async Task<Guid> Add(UserEntity user)
    {
        string sql = @"INSERT INTO 
                        users(name, password, role) 
                        VALUES (@Name, @Password ,@Role) 
                        RETURNING id";

        var queryArguments = new { Name = user.Name, Password = user.Password, Role = UserResponsibilities.Administrator.ToString().ToLower() };

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, queryArguments);
    }
}