using Blazor.Clients.Requests;
using Blazor.Clients.Services;
using Blazor.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor.Clients.Pages;

public sealed partial class ClientsOverview : ComponentBase
{
    [Inject] private IClientsService ClientsService { get; init; } = null!;
    [Inject] private NavigationManager NavManager { get; init; } = null!;

    private List<Client> Clients { get; set; } = [];

    private bool _showEditModal;
    private bool _showDeleteModal;
    private Client? _selectedClient;

    protected override async Task OnInitializedAsync()
    {
        var result = await ClientsService.GetClients();
        result.Switch(
            clients => Clients = clients,
            error => Console.WriteLine(error.Message));
    }

    private void OpenEditModal(Client client)
    {
        _selectedClient = client with { };
        _showEditModal = true;
    }

    private void CloseEditModal()
    {
        _showEditModal = false;
        _selectedClient = null;
        StateHasChanged();
    }

    private async Task HandleClientSaved()
    {
        if (_selectedClient is null) return;


        UpdateClientRequest request = new(
            FirstName: _selectedClient.FirstName,
            MiddleName: _selectedClient.MiddleName,
            LastName: _selectedClient.LastName,
            PhoneNumber: _selectedClient.PhoneNumber,
            HomeAddress: _selectedClient.HomeAddress,
            PassportSeries: _selectedClient.PassportSeries,
            PassportNumber: _selectedClient.PassportNumber);


        var result = await ClientsService.UpdateClient(_selectedClient.Id, request);
        result.Switch(
            _ =>
            {
                var index = Clients.FindIndex(m => m.Id == _selectedClient.Id);
                if (index >= 0)
                {
                    Clients[index] = _selectedClient;
                }
            },
            error => Console.WriteLine(error.Message));

        CloseEditModal();
    }

    private void OpenDeleteModal(Client client)
    {
        _selectedClient = client;
        _showDeleteModal = true;
    }

    private void CloseDeleteModal()
    {
        _showDeleteModal = false;
        _selectedClient = null;
    }

    private async Task HandleDeleteConfirmed()
    {
        if (_selectedClient == null) return;

        var result = await ClientsService.DeleteClient(_selectedClient.Id);
        result.Switch(
            _ => Clients.Remove(_selectedClient),
            error => Console.WriteLine(error.Message));

        CloseDeleteModal();
    }
    
    private void OnDetailsClick(Client client)
    {
        Console.WriteLine("navigating");
        NavManager.NavigateTo($"clients/{client.Id}");
    }
}