using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;

namespace EntityFramework.Api.Endpoints.Clients;

public static class DeleteClientEndpoint
{
    public static void MapDeleteClient(this WebApplication app)
    {
        app.MapDelete("api/v1/clients/{id:guid}",
            async ([FromRoute] Guid id, [FromServices] IClientRepository repository,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await repository.DeleteClientAsync(id, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        _ => Results.NotFound());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.InternalServerError();
                }
            });
    }
}