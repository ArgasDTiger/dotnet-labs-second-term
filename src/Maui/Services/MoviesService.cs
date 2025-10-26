using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Text.Json;
using Maui.Models;
using Maui.RequestModels;
using OneOf.Types;

namespace Maui.Services;

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

    public async Task<OneOf<ImmutableArray<Movie>, ErrorMessage>> GetMovies()
    {
        var response = await _httpClient.GetAsync(MoviesRoute);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
            return error ?? new ErrorMessage("Failed to load movies.");
        }

        var movies = await response.Content.ReadFromJsonAsync<ImmutableArray<Movie>?>(SerializerOptions);
        
        return movies ?? ImmutableArray<Movie>.Empty; 
    }

    public async Task<OneOf<None, ErrorMessage>> CreateMovie(Guid movieId, CreateMovieRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PostAsync(MoviesRoute + movieId, httpContent);
        
        if (response.IsSuccessStatusCode) return new None();
        
        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage("Failed to create movie.");
    }
    
    public async Task<OneOf<None, ErrorMessage>> UpdateMovie(Guid movieId, UpdateMovieRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PutAsync(MoviesRoute + movieId, httpContent);
        
        if (response.IsSuccessStatusCode) return new None();

        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage($"Failed to update movie.");
    }

    public async Task<OneOf<None, ErrorMessage>> DeleteMovie(Guid movieId)
    {
        var response = await _httpClient.DeleteAsync(MoviesRoute + movieId);
        
        if (response.IsSuccessStatusCode) return new None();
        
        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage($"Failed to delete movie.");
    }
}