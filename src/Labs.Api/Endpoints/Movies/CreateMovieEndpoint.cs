using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.Movie;

namespace EntityFramework.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public static void MapCreateMovie(this WebApplication app)
    {
        app.MapPost("api/v1/mvoies",
            async ([FromServices] IMovieRepository movieRepository, [FromBody] CreateMovieRequest request,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    Guid createdMovieId = await movieRepository.AddMovieAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/movies/{createdMovieId}", null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.InternalServerError();
                }
            });
    }
}