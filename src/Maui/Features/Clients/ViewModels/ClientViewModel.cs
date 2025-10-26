using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Maui.Models;
using Maui.Services;

namespace Maui.ViewModels;

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
}