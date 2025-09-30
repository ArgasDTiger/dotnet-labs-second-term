using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.Movie;

namespace EntityFramework.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public static void MapUpdateMovie(this WebApplication app)
    {
        app.MapPut("api/v1/movies/{id:Guid}", async ([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
            CancellationToken cancellationToken, [FromServices] IMovieRepository movieRepository) =>
        {
            try
            {
                var result = await movieRepository.UpdateMovieAsync(id, request, cancellationToken);
                return result.Match(
                    Results.Ok,
                    Results.NotFound);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}