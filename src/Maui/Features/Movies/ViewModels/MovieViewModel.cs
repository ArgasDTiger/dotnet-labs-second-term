using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Maui.Models;
using Maui.Services;

namespace Maui.ViewModels;

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
}