using Blazor.Core.Models;
using Blazor.Movies.Requests;
using Blazor.Movies.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Movies.Pages;

public sealed partial class MoviesOverview : ComponentBase
{
    [Inject] private IMoviesService MoviesService { get; init; } = null!;

    private List<Movie> Movies { get; set; } = [];
    
    private bool _showEditModal;
    private bool _showDeleteModal;
    private Movie? _selectedMovie;

    protected override async Task OnInitializedAsync()
    {
        var result = await MoviesService.GetMovies();
        result.Switch(
            movies => Movies = movies,
            error => Console.WriteLine(error.Message)); // TODO: Handle error
    }

    private void OpenEditModal(Movie movie)
    {
        _selectedMovie = movie with { };
        _showEditModal = true;
    }

    private void CloseEditModal()
    {
        _showEditModal = false;
        _selectedMovie = null;
        StateHasChanged();
    }

    private async Task HandleMovieSaved()
    {
        if (_selectedMovie is null) return;

        
        UpdateMovieRequest request = new(_selectedMovie.Title, _selectedMovie.Description, _selectedMovie.PricePerDay, _selectedMovie.CollateralValue);
        
        var result = await MoviesService.UpdateMovie(_selectedMovie.Id, request);
        result.Switch(
            _ => 
            {
                var index = Movies.FindIndex(m => m.Id == _selectedMovie.Id);
                if (index >= 0)
                {
                    Movies[index] = _selectedMovie;
                }
            },
            error => Console.WriteLine(error.Message));
        
        CloseEditModal();
    }

    private void OpenDeleteModal(Movie movie)
    {
        _selectedMovie = movie;
        _showDeleteModal = true;
    }

    private void CloseDeleteModal()
    {
        _showDeleteModal = false;
        _selectedMovie = null;
    }

    private async Task HandleDeleteConfirmed()
    {
        if (_selectedMovie == null) return;

        var result = await MoviesService.DeleteMovie(_selectedMovie.Id);
        result.Switch(
            _ => Movies.Remove(_selectedMovie),
            error => Console.WriteLine(error.Message));
        
        CloseDeleteModal();
    }
}