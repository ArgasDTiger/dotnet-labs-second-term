using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.ClientMovie;

namespace EntityFramework.Api.Endpoints.Movies;

public static class ReturnMovieEndpoint
{
    public static void MapReturnMovie(this WebApplication app)
    {
        app.MapPost("/api/v1/movies/{movieId:guid}/return", async ([FromRoute] Guid movieId, [FromQuery] Guid clientId,
            [FromServices] IMovieRepository movieRepository, CancellationToken cancellationToken) =>
        {
            try
            {
                ReturnMovieRequest request = new()
                {
                    ClientId = clientId,
                    MovieId = movieId,
                };
                var result = await movieRepository.ReturnMovieAsync(request, cancellationToken);
                return result.Match(
                    _ => Results.NoContent(),
                    error => Results.BadRequest(error: error.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}