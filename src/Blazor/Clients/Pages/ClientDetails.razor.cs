using Blazor.Clients.Services;
using Blazor.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor.Clients.Pages;

public sealed partial class ClientDetails : ComponentBase
{
    [Inject] private IClientsService ClientsService { get; init; } = null!;
    [Parameter] public Guid ClientId { get; init; }

    private Client? Client { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await ClientsService.GetClientById(ClientId);
        result.Switch(
            client => Client = client,
            error => Console.WriteLine(error.Message));
    }
}