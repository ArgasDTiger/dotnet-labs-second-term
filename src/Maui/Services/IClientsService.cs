using System.Collections.Immutable;
using Maui.Models;
using Maui.RequestModels.Client;
using OneOf.Types;

namespace Maui.Services;

public interface IClientsService
{
    Task<OneOf<ImmutableArray<Client>, ErrorMessage>> GetClients();
    Task<OneOf<None, ErrorMessage>> CreateClient(Guid clientId, CreateClientRequest request);
    Task<OneOf<None, ErrorMessage>> UpdateClient(Guid clientId, UpdateClientRequest request);
    Task<OneOf<None, ErrorMessage>> DeleteClient(Guid clientId);
}