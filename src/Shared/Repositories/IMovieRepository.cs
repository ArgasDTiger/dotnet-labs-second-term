using System.Collections.Immutable;
using Shared.Responses;

namespace Shared.Repositories;

public interface IMovieRepository
{
    Task<MovieResponse?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken);
    Task<ImmutableArray<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken);
}