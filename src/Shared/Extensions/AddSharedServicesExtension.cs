using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Settings;

namespace Shared.Extensions;

public static class AddSharedServicesExtension
{
    public static void AddSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ConnectionStringSettings>()
            .Bind(configuration.GetSection(ConnectionStringSettings.SectionName))
            .ValidateDataAnnotations();
    }
}