using EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.Extensions;

public static class AddEntityFrameworkExtension
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MoviesRentContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));
    }
}