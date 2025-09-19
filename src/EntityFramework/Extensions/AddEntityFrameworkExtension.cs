using EntityFramework.Data;
using EntityFramework.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Repositories;

namespace EntityFramework.Extensions;

public static class AddEntityFrameworkExtension
{
    public static void AddEntityFramework(this IServiceCollection services)
    {
        services.AddDbContext<MoviesRentContext>();
        
        services.AddScoped<IMovieRepository, EfMovieRepository>();
        services.AddScoped<IClientRepository, EfClientRepository>();
    }
}