using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMarketService, MarketService>();
        services.AddTransient<AuthenticateService>();
        services.AddTransient<UserService>();
    }
}
