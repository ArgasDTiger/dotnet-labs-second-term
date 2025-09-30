using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.Client;

namespace EntityFramework.Api.Endpoints.Clients;

public static class CreateClientEndpoint
{
    public static void MapCreateClient(this WebApplication app)
    {
        app.MapPost("api/v1/clients",
            async ([FromBody] CreateClientRequest request, [FromServices] IClientRepository repository,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    Guid createdClientId = await repository.AddClientAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/clients/{createdClientId}", null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
    }
}