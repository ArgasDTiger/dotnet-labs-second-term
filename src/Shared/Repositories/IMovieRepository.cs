using Shared.Responses;

namespace Shared.Repositories;

public interface IMovieRepository
{
    Task<MovieResponse?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken);
    Task<List<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken);
}