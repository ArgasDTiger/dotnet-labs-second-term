using EntityFramework.Data;
using EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Repositories;

namespace EntityFramework.Extensions;

public static class AddEntityFrameworkExtension
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MoviesRentContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IMovieRepository, EfMovieRepository>();
        services.AddScoped<IClientRepository, EfClientRepository>();
    }
}