using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.Features.Clients.Models;
using Maui.Features.Clients.Services;
using Maui.Features.Movies.Services;
using Maui.Shared.ViewModels;

namespace Maui.Features.Clients.ViewModels;

[QueryProperty(nameof(Client), nameof(Client))]
public sealed partial class ClientDetailsViewModel : ViewModel
{
    private readonly IClientsService _clientsService;
    private readonly IMoviesService _moviesService;

    [ObservableProperty]
    private Client _client;
    
    public ClientDetailsViewModel(IClientsService clientsService, IMoviesService moviesService)
    {
        _clientsService = clientsService;
        _moviesService = moviesService;
    }
    
    partial void OnClientChanged(Client value)
    {
        if (value is not null)
        {
            LoadClientDetailsCommand.ExecuteAsync(value.Id);
        }
    }

    [RelayCommand]
    private async Task LoadClientDetailsAsync(Guid clientId)
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var result = await _clientsService.GetClientById(clientId);
            result.Switch(
                client => Client = client,
                error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading client details: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Could not load client details.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ReturnMovieAsync(ClientMovie movie)
    {
        if (movie is null) return;

        try
        {
            var result = await _moviesService.ReturnMovie(movie.MovieId, Client.Id);
            result.Switch(
                _ =>
                {
                    LoadClientDetailsCommand.ExecuteAsync(Client.Id);
                },
                error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error returning movie: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Could not return movie.", "OK");
        }
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}