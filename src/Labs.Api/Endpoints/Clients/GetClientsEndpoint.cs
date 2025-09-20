using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;

namespace EntityFramework.Api.Endpoints.Clients;

public static class GetClientsEndpoint
{
    public static void MapGetClients(this WebApplication app)
    {
        app.MapGet("api/v1/clients",
            async ([FromServices] IClientRepository clientRepository, CancellationToken cancellationToken) =>
            {
                try
                {
                    return Results.Ok(await clientRepository.GetAllClientsAsync(cancellationToken));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.InternalServerError();
                }
            });
    }
}