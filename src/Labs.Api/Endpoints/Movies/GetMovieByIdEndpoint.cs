using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Responses;

namespace EntityFramework.Api.Endpoints.Movies;

public static class GetMovieByIdEndpoint
{
    public static void MapGetMovieById(this WebApplication app)
    {
        app.MapGet("api/v1/movies/{id:guid}",
            async ([FromServices] IMovieRepository movieRepository,
                [FromRoute] Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    MovieResponse? movie = await movieRepository.GetMovieByIdAsync(id, cancellationToken);
                    return movie is null ? Results.NotFound() : Results.Ok(movie);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.InternalServerError();
                }
                
            });
    }
}