using Blazor.Clients.Requests;
using Blazor.Core.Models;
using Blazor.Shared.Results;

namespace Blazor.Clients.Services;

public interface IClientsService
{
    Task<OneOf<List<Client>, Error>> GetClients();
    Task<OneOf<Success, Error>> CreateClient(CreateClientRequest request);
    Task<OneOf<Success, Error>> UpdateClient(Guid clientId, UpdateClientRequest request);
    Task<OneOf<Success, Error>> DeleteClient(Guid clientId);
    Task<OneOf<Client, Error>> GetClientById(Guid clientId);
}