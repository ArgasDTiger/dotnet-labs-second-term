using System.Collections.Immutable;
using Shared.Responses;

namespace Shared.Repositories;

public interface IClientRepository
{
    Task<ClientWithMoviesResponse?> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken);
    Task<ImmutableArray<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken);
}