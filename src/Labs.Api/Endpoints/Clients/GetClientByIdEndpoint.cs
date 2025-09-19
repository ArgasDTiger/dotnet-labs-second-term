using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Responses;

namespace EntityFramework.Api.Endpoints.Clients;

public static class GetClientByIdEndpoint
{
    public static void MapGetClientById(this WebApplication app)
    {
        app.MapGet("api/v1/clients/{id:guid}",
            async ([FromServices] IClientRepository clientRepository, [FromRoute] Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    ClientWithMoviesResponse? client = await clientRepository.GetClientByIdAsync(id, cancellationToken);
                    return client is null ? Results.NotFound() : Results.Ok(client);
                }
                catch (Exception)
                {
                    return Results.InternalServerError();
                }
            });
    }
}