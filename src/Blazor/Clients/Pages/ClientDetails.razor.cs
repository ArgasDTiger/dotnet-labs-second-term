using Blazor.Clients.Services;
using Blazor.Core.Models;
using Blazor.Movies.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Clients.Pages;

public sealed partial class ClientDetails : ComponentBase
{
    [Inject] private IClientsService ClientsService { get; init; } = null!;
    [Inject] private IMoviesService MoviesService { get; init; } = null!;
    [Inject] private NavigationManager NavManager { get; init; } = null!;
    [Parameter] public Guid ClientId { get; init; }

    private Client? Client { get; set; }
    private bool _showReturnModal;
    private ClientMovie? _selectedMovie;

    protected override async Task OnInitializedAsync()
    {
        await LoadClient();
    }

    private async Task LoadClient()
    {
        var result = await ClientsService.GetClientById(ClientId);
        result.Switch(
            client => Client = client,
            error => Console.WriteLine(error.Message));
    }

    private void OpenReturnModal(ClientMovie movie)
    {
        _selectedMovie = movie;
        _showReturnModal = true;
    }

    private void CloseReturnModal()
    {
        _showReturnModal = false;
        _selectedMovie = null;
    }

    private async Task HandleReturnConfirmed()
    {
        if (_selectedMovie == null || Client == null) return;

        var result = await MoviesService.ReturnMovie(_selectedMovie.MovieId, ClientId);
    
        var isSuccess = result.Match(
            _ => true,
            error =>
            {
                Console.WriteLine(error.Message);
                return false;
            });

        if (isSuccess)
        {
            await LoadClient();
        }

        CloseReturnModal();
    }
    
    private void GoBack()
    {
        NavManager.NavigateTo("clients");
    }
}