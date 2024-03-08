using MarketPlaceApi.Clients;
using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Repositories;
using MarketPlaceApi.Services;
using MarketPlaceApi.Validators;
using Npgsql;
using System.Data;

namespace MarketPlaceApi;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Postgre connection
        var connectionString = configuration.GetConnectionString("PostgreConnection") ?? throw new ArgumentNullException("PostgreSQL connection string was not found.");
        services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));

        services.AddTransient<IMarketRepository, MarketRepository>();
        services.AddTransient<IMarketService, MarketService>();

        //Validator registration
        services.AddTransient<IItemDtoValidator, ItemDtoValidator>();
        services.AddTransient<IOrderDtoValidator, OrderDtoValidator>();

        //Client registration
        services.AddTransient<IUserClient, UserClient>();
        services.AddHttpClient();

    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
