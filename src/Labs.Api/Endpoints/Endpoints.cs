using EntityFramework.Api.Endpoints.Clients;
using EntityFramework.Api.Endpoints.Movies;

namespace EntityFramework.Api.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        // Movies
        app.MapGetMovieById();
        app.MapGetMovies();
        
        // Clients
        app.MapGetClientById();
        app.MapGetClients();
    }
}