using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.ClientMovie;

namespace EntityFramework.Api.Endpoints.Movies;

public static class RentMovieEndpoint
{
    public static void MapRentMovie(this WebApplication app)
    {
        // TODO: put movieId in body? because immutability
        app.MapPost("/api/v1/movies/{movieId:guid}/rent", async ([FromRoute] Guid movieId, [FromQuery] Guid clientId,
            [FromQuery] DateTime expectedReturnDate, [FromServices] IMovieRepository movieRepository,
            CancellationToken cancellationToken) =>
        {
            try
            {
                RentMovieRequest request = new RentMovieRequest
                {
                    ClientId = clientId,
                    MovieId = movieId,
                    ExpectedReturnDate = expectedReturnDate
                };
                var result = await movieRepository.RentMovieAsync(request, cancellationToken);
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