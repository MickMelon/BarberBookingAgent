using BarberBookingAgent.WebApi.Options;
using BarberBookingAgent.WebApi.Plugins;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace BarberBookingAgent.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services)
    {
        services.AddScoped<Kernel>(serviceProvider =>
        {
            OpenAiOptions options = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;

            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.Services.AddLogging(_ => _
                .SetMinimumLevel(LogLevel.Trace)
                .AddDebug()
                .AddConsole());

            kernelBuilder.Services.AddOpenAIChatCompletion(options.ModelId, options.ApiKey, options.OrganizationId);

            kernelBuilder.Plugins.AddFromType<BookingPlugin>();

            Kernel kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }
}