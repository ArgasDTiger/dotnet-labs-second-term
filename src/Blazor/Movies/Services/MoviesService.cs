using System.Text.Json;
using Blazor.Core.Models;
using Blazor.Movies.Requests;
using Blazor.Shared.Results;

namespace Blazor.Movies.Services;

public sealed class MoviesService : IMoviesService
{
    private readonly HttpClient _httpClient;
    private const string MoviesRoute = "movies/";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MoviesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OneOf<List<Movie>, Error>> GetMovies()
    {
        var response = await _httpClient.GetAsync(MoviesRoute);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
            return error ?? new Error("Failed to load movies.");
        }

        var movies = await response.Content.ReadFromJsonAsync<List<Movie>?>(SerializerOptions);

        return movies ?? [];
    }

    public async Task<OneOf<Success, Error>> CreateMovie(CreateMovieRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PostAsync(MoviesRoute, httpContent);

        if (response.IsSuccessStatusCode) return StaticResults.Success;

        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error("Failed to create movie.");
    }

    public async Task<OneOf<Success, Error>> UpdateMovie(Guid movieId, UpdateMovieRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PutAsync(MoviesRoute + movieId, httpContent);

        if (response.IsSuccessStatusCode) return StaticResults.Success;

        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error($"Failed to update movie.");
    }

    public async Task<OneOf<Success, Error>> DeleteMovie(Guid movieId)
    {
        var response = await _httpClient.DeleteAsync(MoviesRoute + movieId);

        if (response.IsSuccessStatusCode) return StaticResults.Success;

        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error($"Failed to delete movie.");
    }

    public async Task<OneOf<Success, Error>> ReturnMovie(Guid movieId, Guid clientId)
    {
        var response = await _httpClient.PostAsync($"{MoviesRoute}{movieId}/return?clientId={clientId}", null);

        if (response.IsSuccessStatusCode) return StaticResults.Success;

        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error("Failed to return movie.");
    }
}