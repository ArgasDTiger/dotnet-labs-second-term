using Blazor.Core.Models;
using Blazor.Movies.Requests;
using Blazor.Shared.Results;

namespace Blazor.Movies.Services;

public interface IMoviesService
{
    Task<OneOf<List<Movie>, Error>> GetMovies();
    Task<OneOf<Success, Error>> CreateMovie(CreateMovieRequest request);
    Task<OneOf<Success, Error>> UpdateMovie(Guid movieId, UpdateMovieRequest request);
    Task<OneOf<Success, Error>> DeleteMovie(Guid movieId);
    Task<OneOf<Success, Error>> ReturnMovie(Guid movieId, Guid clientId);
}