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
    private bool _isNewMovie;
    private Movie? _selectedMovie;

    protected override async Task OnInitializedAsync()
    {
        var result = await MoviesService.GetMovies();
        result.Switch(
            movies => Movies = movies,
            error => Console.WriteLine(error.Message)); // TODO: Handle error
    }

    private void OpenNewMovieModal()
    {
        _selectedMovie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = string.Empty,
            Description = string.Empty,
            PricePerDay = 0,
            CollateralValue = 0,
        };
        _isNewMovie = true;
        _showEditModal = true;
    }

    private void OpenEditModal(Movie movie)
    {
        _selectedMovie = movie with { };
        _isNewMovie = false;
        _showEditModal = true;
    }

    private void CloseEditModal()
    {
        _showEditModal = false;
        _selectedMovie = null;
        _isNewMovie = false;
        StateHasChanged();
    }

    private async Task HandleMovieSaved()
    {
        if (_selectedMovie is null) return;

        if (_isNewMovie)
        {
            CreateMovieRequest request = new(
                _selectedMovie.Title, 
                _selectedMovie.Description, 
                _selectedMovie.CollateralValue,
                _selectedMovie.PricePerDay);
            
            var result = await MoviesService.CreateMovie(request);
            result.Switch(
                _ => Movies.Add(_selectedMovie),
                error => Console.WriteLine(error.Message));
        }
        else
        {
            UpdateMovieRequest request = new(
                _selectedMovie.Title, 
                _selectedMovie.Description, 
                _selectedMovie.PricePerDay, 
                _selectedMovie.CollateralValue);
            
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
        }
        
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