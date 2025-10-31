using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Maui.Features.Clients.Models;
using Maui.Features.Clients.Services;
using Maui.Features.Clients.Views;
using Maui.Shared.ViewModels;

namespace Maui.Features.Clients.ViewModels;

public sealed partial class ClientViewModel : ViewModel
{
    private readonly IClientsService _clientsService;

    public ObservableCollection<Client> Clients { get; set; } = [];

    public ClientViewModel(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [RelayCommand]
    private async Task LoadClientsAsync()
    {
        if (IsLoading) return; // Add this guard
        try
        {
            IsLoading = true;
            var result = await _clientsService.GetClients();
            result.Switch(
                clients =>
                {
                    Clients.Clear();
                    foreach (var client in clients)
                    {
                        Clients.Add(client);
                    }
                },
                error =>
                {
                    Debug.WriteLine($"Failed to get clients: {error.Message}");
                    Shell.Current.DisplayAlert("Error!", error.Message, "OK");
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get clients: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    [RelayCommand]
    private async Task GoToDetailsAsync(Client client)
    {
        await Shell.Current.GoToAsync($"{nameof(ClientsDetailsPage)}", true,
            new Dictionary<string, object>
            {
                { "Client", client }
            });
    }

    [RelayCommand]
    private async Task DeleteClientAsync(Client client)
    {
        if (client is null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete {client.FirstName} {client.LastName}?",
            "Yes", "No");

        if (!confirmed) return;

        try
        {
            var result = await _clientsService.DeleteClient(client.Id);
            result.Switch(
                _ =>
                {
                    Clients.Remove(client);
                },
                error =>
                {
                    Debug.WriteLine($"Failed to delete client: {error.Message}");
                    Shell.Current.DisplayAlert("Error!", error.Message, "OK");
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to delete client: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task GoToUpdateClientAsync(Client client)
    {
        if (client is null) return;
        
        await Shell.Current.GoToAsync($"{nameof(ClientUpdatePage)}", true,
            new Dictionary<string, object>
            {
                { "Client", client }
            });
    }

    [RelayCommand]
    private async Task GoToCreateClientAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(ClientUpdatePage)}");
    }
}