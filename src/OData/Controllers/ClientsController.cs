using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace OData.Controllers;

public sealed class ClientsController : ODataController
{
    private readonly MoviesRentContext _context;

    public ClientsController(MoviesRentContext context)
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

    public async Task<IActionResult> Post([FromBody] Client client)
    {
        _context.Set<Client>().Add(client);
        await _context.SaveChangesAsync();

        return Created(client);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] Client client)
    {
        if (key != client.Id)
        {
            return BadRequest("Key mismatch");
        }

        var existingClient = await _context.Set<Client>().FindAsync(key);
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

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ClientExists(key))
            {
                return NotFound();
            }
            throw;
        }

        return Updated(existingClient);
    }

    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Client> delta)
    {
        var client = await _context.Set<Client>().FindAsync(key);
        if (client == null)
        {
            return NotFound();
        }

        delta.Patch(client);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ClientExists(key))
            {
                return NotFound();
            }
            throw;
        }

        return Updated(client);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        var client = await _context.Set<Client>().FindAsync(key);
        if (client == null)
        {
            return NotFound();
        }

        _context.Set<Client>().Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ClientExists(Guid key)
    {
        return await _context.Set<Client>().AnyAsync(c => c.Id == key);
    }
}