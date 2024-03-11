using MarketPlaceApi.BackgroundServices;
using MarketPlaceApi.Clients;
using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Repositories;
using MarketPlaceApi.Services;
using MarketPlaceApi.Validators;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Reflection;

namespace MarketPlaceApi;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

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

        //Background service
        services.AddHostedService<UnpaidOrdersCleanupWorkerService>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        //Swagger Documentation
        var info = new OpenApiInfo()
        {
            Title = "Market Place API",
            Version = "v1",
            Description = "Market with some endpoint for creating items and creating orders"
        };

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", info);

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}