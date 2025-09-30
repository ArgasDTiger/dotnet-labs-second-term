using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;

namespace EntityFramework.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public static void MapDeleteMovie(this WebApplication app)
    {
        app.MapDelete("api/v1/movies/{id:guid}", async ([FromRoute] Guid id, [FromServices] IMovieRepository repository, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await repository.DeleteMovieAsync(id, cancellationToken);
                return result.Match(
                    _ => Results.NoContent(),
                    _ => Results.NotFound());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.InternalServerError();
            }
        });
    }
}