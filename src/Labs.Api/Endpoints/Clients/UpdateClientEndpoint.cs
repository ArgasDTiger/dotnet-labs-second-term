using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.Client;

namespace EntityFramework.Api.Endpoints.Clients;

public static class UpdateClientEndpoint
{
    public static void MapUpdateClient(this WebApplication app)
    {
        app.MapPut("api/v1/clients/{id:guid}",
            async ([FromRoute] Guid id, [FromBody] UpdateClientRequest request,
                [FromServices] IClientRepository clientRepository,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await clientRepository.UpdateClientAsync(id, request, cancellationToken);
                    return result.Match(
                        Results.Ok,
                        Results.NotFound);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
    }
}