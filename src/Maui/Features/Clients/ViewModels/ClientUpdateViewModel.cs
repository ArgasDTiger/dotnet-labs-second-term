using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.Features.Clients.Models;
using Maui.Features.Clients.Requests;
using Maui.Features.Clients.Services;
using Maui.Shared.ViewModels;

namespace Maui.Features.Clients.ViewModels;

[QueryProperty(nameof(Client), "Client")]
public sealed partial class ClientUpdateViewModel : ViewModel
{
    private readonly IClientsService _clientsService;
    private bool _isUpdateMode;

    [ObservableProperty] private Client _client;

    [ObservableProperty] private string _pageTitle;

    [ObservableProperty] private string _firstName;
    [ObservableProperty] private string _middleName;
    [ObservableProperty] private string _lastName;
    [ObservableProperty] private string _phoneNumber;
    [ObservableProperty] private string _homeAddress;
    [ObservableProperty] private string? _passportSeries;
    [ObservableProperty] private string _passportNumber;

    public ClientUpdateViewModel(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    partial void OnClientChanged(Client value)
    {
        _isUpdateMode = value is not null;

        if (_isUpdateMode)
        {
            PageTitle = "Update Client";
            FirstName = value.FirstName;
            MiddleName = value.MiddleName;
            LastName = value.LastName;
            PhoneNumber = value.PhoneNumber;
            HomeAddress = value.HomeAddress;
            PassportSeries = value.PassportSeries;
            PassportNumber = value.PassportNumber;
        }
        else
        {
            PageTitle = "Create Client";
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            HomeAddress = string.Empty;
            PassportSeries = string.Empty;
            PassportNumber = string.Empty;
        }
    }

    [RelayCommand]
    private async Task SaveClientAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            if (_isUpdateMode)
            {
                var request = new UpdateClientRequest(
                    FirstName: FirstName,
                    MiddleName: MiddleName,
                    LastName: LastName,
                    PhoneNumber: PhoneNumber,
                    HomeAddress: HomeAddress,
                    PassportSeries: PassportSeries,
                    PassportNumber: PassportNumber);

                var result = await _clientsService.UpdateClient(Client.Id, request);
                result.Switch(
                    _ => GoBackAsync(true),
                    error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
                );
            }
            else
            {
                var request = new CreateClientRequest(
                    FirstName: FirstName,
                    MiddleName: MiddleName,
                    LastName: LastName,
                    PhoneNumber: PhoneNumber,
                    HomeAddress: HomeAddress,
                    PassportSeries: PassportSeries,
                    PassportNumber: PassportNumber);

                var result = await _clientsService.CreateClient(request);
                result.Switch(
                    _ => GoBackAsync(true),
                    error => Shell.Current.DisplayAlert("Error", error.Message, "OK")
                );
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save client: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static async Task GoBackAsync(bool loadClients)
    {
        if (loadClients)
        {
            await Shell.Current.GoToAsync("..?LoadClients=True");
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