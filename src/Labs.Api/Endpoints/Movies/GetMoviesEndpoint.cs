using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Responses;

namespace EntityFramework.Api.Endpoints.Movies;

public static class GetMoviesEndpoint
{
    public static void MapGetMovies(this WebApplication app)
    {
        app.MapGet("api/v1/movies",
            async ([FromServices] IMovieRepository movieRepository, CancellationToken cancellationToken) =>
            {
                try
                {
                    List<MovieResponse> movies = await movieRepository.GetAllMoviesAsync(cancellationToken);
                    return Results.Ok(movies);
                }
                catch (Exception)
                {
                    return Results.InternalServerError();
                }
            });
    }
}