using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.Features.Movies.Models;
using Maui.Features.Movies.Requests;
using Maui.Features.Movies.Services;
using Maui.Shared.ViewModels;

namespace Maui.Features.Movies.ViewModels;

[QueryProperty(nameof(Movie), "Movie")]
public sealed partial class MovieUpdateViewModel : ViewModel
{
    private readonly IMoviesService _moviesService;
    private bool _isUpdateMode;

    [ObservableProperty]
    private Movie _movie;

    [ObservableProperty]
    private string _pageTitle;
    
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private decimal _collateralValue;
    [ObservableProperty]
    private decimal _pricePerDay;

    public MovieUpdateViewModel(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    partial void OnMovieChanged(Movie value)
    {
        _isUpdateMode = value is not null;
        
        if (_isUpdateMode)
        {
            PageTitle = "Update Movie";
            Title = value.Title;
            Description = value.Description;
            CollateralValue = value.CollateralValue;
            PricePerDay = value.PricePerDay;
        }
        else
        {
            PageTitle = "Create Movie";
            Title = string.Empty;
            Description = string.Empty;
            CollateralValue = 0;
            PricePerDay = 0;
        }
    }

    [RelayCommand]
    private async Task SaveMovieAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            if (_isUpdateMode)
            {
                var request = new UpdateMovieRequest(
                    Title: Title, 
                    Description: Description,
                    CollateralValue: CollateralValue,
                    PricePerDay: PricePerDay);
                var result = await _moviesService.UpdateMovie(Movie.Id, request);
                result.Switch(
                    _ => GoBackAsync(true), // Pass true to reload
                    error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
                );
            }
            else
            {
                var request = new CreateMovieRequest(
                    Title: Title,
                    Description: 
                    Description, 
                    CollateralValue:
                    CollateralValue,
                    PricePerDay: PricePerDay);
                
                // --- FIX: Your service needs a Guid here ---
                var result = await _moviesService.CreateMovie(request); 
                
                result.Switch(
                    _ => GoBackAsync(true),
                    error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
                );
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save movie: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private static async Task GoBackAsync(bool loadMovies)
    {
        if (loadMovies)
        {
            await Shell.Current.GoToAsync("..?LoadMovies=True");
        }
        else
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private static async Task CancelAsync()
    {
        await GoBackAsync(false);
    }
}

