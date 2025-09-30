using System.Collections.Immutable;
using Shared.Responses;
using OneOf;
using OneOf.Types;
using Shared.Requests.Movie;

namespace Shared.Repositories;

public interface IMovieRepository
{
    Task<MovieResponse?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken);
    Task<ImmutableArray<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken);
    Task<Guid> AddMovieAsync(CreateMovieRequest movie, CancellationToken cancellationToken);
    Task<OneOf<None, NotFound>> UpdateMovieAsync(Guid movieId, UpdateMovieRequest movie, CancellationToken cancellationToken);
    Task<OneOf<None, NotFound>> DeleteMovieAsync(Guid movieId, CancellationToken cancellationToken);
}