using Maui.Constants;

namespace Maui.Shared.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddApiHttpClient<TClient, TImplementation>(this IServiceCollection services)
        where TClient : class where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>(client =>
        {
            client.BaseAddress = new Uri(ApiConstants.BaseUrl);
        });

        return services;
    }
}