using System.Collections.Immutable;
using Maui.Features.Movies.Models;
using Maui.Features.Movies.Requests;
using Maui.Shared.Models;
using OneOf.Types;

namespace Maui.Features.Movies.Services;

public interface IMoviesService
{
    Task<OneOf<ImmutableArray<Movie>, ErrorMessage>> GetMovies();
    Task<OneOf<None, ErrorMessage>> CreateMovie(CreateMovieRequest request);
    Task<OneOf<None, ErrorMessage>> UpdateMovie(Guid movieId, UpdateMovieRequest request);
    Task<OneOf<None, ErrorMessage>> DeleteMovie(Guid movieId);
    Task<OneOf<None, ErrorMessage>> ReturnMovie(Guid movieId, Guid clientId);
}