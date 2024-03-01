using BarberBookingAgent.Application.Chat;
using BarberBookingAgent.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBookingAgent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ChatMessageHandler>();
        services.AddSemanticKernel(configuration);
        services.AddSingleton<ChatHistoryStorage>();

        return services;
    }

    private static IServiceCollection AddSemanticKernel(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        

        return services;
    }
}
