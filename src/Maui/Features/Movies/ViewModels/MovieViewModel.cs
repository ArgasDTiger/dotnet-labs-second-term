using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Maui.Features.Movies.Models;
using Maui.Features.Movies.Services;
using Maui.Features.Movies.Views; // Need this for navigation
using Maui.Shared.ViewModels;

namespace Maui.Features.Movies.ViewModels;

public sealed partial class MovieViewModel : ViewModel
{
    private readonly IMoviesService _moviesService;

    public ObservableCollection<Movie> Movies { get; set; } = [];

    public MovieViewModel(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [RelayCommand]
    private async Task LoadMoviesAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var result = await _moviesService.GetMovies();
            result.Switch(
                movies =>
                {
                    Movies.Clear();
                    foreach (var movie in movies)
                    {
                        Movies.Add(movie);
                    }
                },
                error =>
                {
                    Debug.WriteLine($"Failed to get movies: {error.Message}");
                    Shell.Current.DisplayAlert("Error!", error.Message, "OK");
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get movies: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteMovieAsync(Movie movie)
    {
        if (movie is null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete \"{movie.Title}\"?",
            "Yes", "No");

        if (!confirmed) return;

        try
        {
            var result = await _moviesService.DeleteMovie(movie.Id);
            result.Switch(
                _ =>
                {
                    Movies.Remove(movie);
                },
                error =>
                {
                    Debug.WriteLine($"Failed to delete movie: {error.Message}");
                    Shell.Current.DisplayAlert("Error!", error.Message, "OK");
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to delete movie: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task GoToUpdateMovieAsync(Movie movie)
    {
        if (movie is null) return;
        
        await Shell.Current.GoToAsync($"{nameof(MovieUpdatePage)}", true,
            new Dictionary<string, object>
            {
                { "Movie", movie }
            });
    }

    [RelayCommand]
    private async Task GoToCreateMovieAsync()
    {
        // Go to the update page, but pass no movie (so it knows to be in "Create" mode)
        await Shell.Current.GoToAsync($"{nameof(MovieUpdatePage)}");
    }
}