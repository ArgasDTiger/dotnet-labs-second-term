using AdoNet.DatabaseProvider;
using AdoNet.Repositories;
using Shared.Repositories;

namespace AdoNet.Extensions;

public static class AddAdoExtensions
{
    public static void AddAdoServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseConnection, DatabaseConnection>();

        services.AddScoped<IMovieRepository, AdoMovieRepository>();
        services.AddScoped<IClientRepository, AdoClientRepository>();
    }
}