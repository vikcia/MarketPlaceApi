using MarketPlaceApi.BackgroundServices;
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
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Postgre connection
        var connectionString = configuration.GetConnectionString("PostgreConnection") ?? throw new ArgumentNullException("PostgreSQL connection string was not found.");
        services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));

        services.AddTransient<IMarketRepository, MarketRepository>();
        services.AddTransient<IMarketService, MarketService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<AuthenticateService>();
        services.AddTransient<UserService>();

        //Validator registration
        services.AddTransient<IItemDtoValidator, ItemDtoValidator>();
        services.AddTransient<IOrderDtoValidator, OrderDtoValidator>();

        //Client registration
        services.AddTransient<IUserClient, UserClient>();
        services.AddHttpClient();

        //Background service
        services.AddHostedService<UnpaidOrdersCleanupWorkerService>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}