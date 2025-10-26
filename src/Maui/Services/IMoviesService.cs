using System.Collections.Immutable;
using Maui.Models;
using Maui.RequestModels;
using OneOf.Types;

namespace Maui.Services;

public interface IMoviesService
{
    Task<OneOf<ImmutableArray<Movie>, ErrorMessage>> GetMovies();
    Task<OneOf<None, ErrorMessage>> CreateMovie(Guid movieId, CreateMovieRequest request);
    Task<OneOf<None, ErrorMessage>> UpdateMovie(Guid movieId, UpdateMovieRequest request);
    Task<OneOf<None, ErrorMessage>> DeleteMovie(Guid movieId);
}