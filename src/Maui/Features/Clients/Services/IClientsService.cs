using System.Collections.Immutable;
using Maui.Features.Clients.Models;
using Maui.Features.Clients.Requests;
using Maui.Shared.Models;
using OneOf.Types;

namespace Maui.Features.Clients.Services;

public interface IClientsService
{
    Task<OneOf<ImmutableArray<Client>, ErrorMessage>> GetClients();
    Task<OneOf<None, ErrorMessage>> CreateClient(CreateClientRequest request);
    Task<OneOf<None, ErrorMessage>> UpdateClient(Guid clientId, UpdateClientRequest request);
    Task<OneOf<None, ErrorMessage>> DeleteClient(Guid clientId);
    Task<OneOf<Client, ErrorMessage>> GetClientById(Guid clientId);
}