using System.Collections.Immutable;
using Shared.Requests.Client;
using Shared.Responses;
using OneOf;
using OneOf.Types;

namespace Shared.Repositories;

public interface IClientRepository
{
    Task<ClientWithMoviesResponse?> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken);
    Task<ImmutableArray<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken);
    Task<Guid> AddClientAsync(CreateClientRequest client, CancellationToken cancellationToken);
    Task<OneOf<None, NotFound>> UpdateClientAsync(Guid clientId, UpdateClientRequest client, CancellationToken cancellationToken);
    Task<OneOf<None, NotFound>> DeleteClientAsync(Guid clientId, CancellationToken cancellationToken);
}