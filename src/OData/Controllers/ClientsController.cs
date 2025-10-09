using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Shared.Entities;
using Shared.Requests.Client;

namespace OData.Controllers;

public sealed class ClientsController : BaseController
{
    private readonly MoviesRentContext _context;

    public ClientsController(MoviesRentContext context) : base(context)
    {
        _context = context;
    }

    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    public IQueryable<Client> Get()
    {
        return _context.Set<Client>();
    }

    [EnableQuery]
    public SingleResult<Client> Get([FromRoute] Guid key)
    {
        return SingleResult.Create(_context.Set<Client>().Where(c => c.Id == key));
    }

    public async Task<IActionResult> Post([FromBody] CreateClientRequest client, CancellationToken cancellationToken)
    {
        if (IsInvalidModel(out var errors))
        {
            return BadRequest(errors); 
        } 
        Client newClient = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = client.FirstName,
            MiddleName = client.MiddleName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber,
            HomeAddress = client.HomeAddress,
            PassportSeries = client.PassportSeries,
            PassportNumber = client.PassportNumber
        };

        _context.Set<Client>().Add(newClient);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return Created(client);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateClientRequest client,
        CancellationToken cancellationToken)
    {
        if (IsInvalidModel(out var errors))
        {
            return BadRequest(errors); 
        }
        var existingClient = await _context.Set<Client>().FindAsync([key], cancellationToken);
        if (existingClient == null)
        {
            return NotFound();
        }

        existingClient.FirstName = client.FirstName;
        existingClient.MiddleName = client.MiddleName;
        existingClient.LastName = client.LastName;
        existingClient.PhoneNumber = client.PhoneNumber;
        existingClient.HomeAddress = client.HomeAddress;
        existingClient.PassportSeries = client.PassportSeries;
        existingClient.PassportNumber = client.PassportNumber;

        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);
        return Updated(existingClient);
    }

    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Client> delta,
        CancellationToken cancellationToken)
    {
        var client = await _context.Set<Client>().FindAsync([key], cancellationToken);
        if (client == null)
        {
            return NotFound();
        }

        delta.Patch(client);

        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);
        return Updated(client);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key, CancellationToken cancellationToken)
    {
        var client = await _context.Set<Client>().FindAsync([key], cancellationToken);
        if (client == null)
        {
            return NotFound();
        }

        _context.Set<Client>().Remove(client);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}