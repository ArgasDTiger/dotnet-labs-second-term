using Shared.Responses;

namespace Shared.Repositories;

public interface IClientRepository
{
    Task<ClientWithMoviesResponse?> GetClientByIdAsync(int clientId, CancellationToken cancellationToken);
    Task<List<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken);
}