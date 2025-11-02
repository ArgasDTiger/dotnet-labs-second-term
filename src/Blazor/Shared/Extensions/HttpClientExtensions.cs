namespace Blazor.Shared.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddApiHttpClient<TClient, TImplementation>(this IServiceCollection services,
        IConfiguration configuration)
        where TClient : class where TImplementation : class, TClient
    {
        string apiUrl = configuration["ApiUrl"] ?? throw new Exception("ApiUrl is not set.");
        services.AddHttpClient<TClient, TImplementation>(client =>
        {
            client.BaseAddress = new Uri(apiUrl);
        });

        return services;
    }
}