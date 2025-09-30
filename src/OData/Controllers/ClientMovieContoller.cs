using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace OData.Controllers;

public sealed class ClientMoviesController : ODataController
{
    private readonly MoviesRentContext _context;

    public ClientMoviesController(MoviesRentContext context)
    {
        _context = context;
    }

    public IQueryable<ClientMovie> Get()
    {
        return _context.Set<ClientMovie>();
    }

    [EnableQuery]
    public SingleResult<ClientMovie> Get([FromRoute] Guid key)
    {
        return SingleResult.Create(_context.Set<ClientMovie>().Where(cm => cm.Id == key));
    }

    public async Task<IActionResult> Post([FromBody] ClientMovie clientMovie)
    {
        bool clientExists = await _context.Set<Client>().AnyAsync(c => c.Id == clientMovie.ClientId);
        bool movieExists = await _context.Set<Movie>().AnyAsync(m => m.Id == clientMovie.MovieId);

        if (!clientExists)
        {
            return BadRequest("Client does not exist");
        }

        if (!movieExists)
        {
            return BadRequest("Movie does not exist");
        }

        _context.Set<ClientMovie>().Add(clientMovie);
        await _context.SaveChangesAsync();

        return Created(clientMovie);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] ClientMovie clientMovie)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (key != clientMovie.Id)
        {
            return BadRequest("Key mismatch");
        }

        var existingClientMovie = await _context.Set<ClientMovie>().FindAsync(key);
        if (existingClientMovie == null)
        {
            return NotFound();
        }

        // Validate that Client and Movie exist
        var clientExists = await _context.Set<Client>().AnyAsync(c => c.Id == clientMovie.ClientId);
        var movieExists = await _context.Set<Movie>().AnyAsync(m => m.Id == clientMovie.MovieId);

        if (!clientExists)
        {
            return BadRequest("Client does not exist");
        }

        if (!movieExists)
        {
            return BadRequest("Movie does not exist");
        }

        existingClientMovie.StartDate = clientMovie.StartDate;
        existingClientMovie.ExpectedReturnDate = clientMovie.ExpectedReturnDate;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ClientMovieExists(key))
            {
                return NotFound();
            }
            throw;
        }

        return Updated(existingClientMovie);
    }

    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<ClientMovie> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var clientMovie = await _context.Set<ClientMovie>().FindAsync(key);
        if (clientMovie == null)
        {
            return NotFound();
        }

        delta.Patch(clientMovie);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ClientMovieExists(key))
            {
                return NotFound();
            }
            throw;
        }

        return Updated(clientMovie);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        var clientMovie = await _context.Set<ClientMovie>().FindAsync(key);
        if (clientMovie == null)
        {
            return NotFound();
        }

        _context.Set<ClientMovie>().Remove(clientMovie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ClientMovieExists(Guid key)
    {
        return await _context.Set<ClientMovie>().AnyAsync(cm => cm.Id == key);
    }
}