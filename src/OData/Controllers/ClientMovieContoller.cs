using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Requests.ClientMovie;

namespace OData.Controllers;

public sealed class ClientMoviesController : BaseController
{
    private readonly MoviesRentContext _context;

    public ClientMoviesController(MoviesRentContext context) : base(context)
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

    public async Task<IActionResult> Post([FromBody] CreateClientMovieRequest clientMovie,
        CancellationToken cancellationToken)
    {
        if (IsInvalidModel(out var errors))
        {
            return BadRequest(errors); 
        }
        bool clientExists = await _context.Set<Client>().AnyAsync(c => c.Id == clientMovie.ClientId, cancellationToken);
        bool movieExists = await _context.Set<Movie>().AnyAsync(m => m.Id == clientMovie.MovieId, cancellationToken);

        if (!clientExists)
        {
            return BadRequest("Client does not exist");
        }

        if (!movieExists)
        {
            return BadRequest("Movie does not exist");
        }

        ClientMovie newClientMovie = new ClientMovie
        {
            Id = Guid.NewGuid(),
            MovieId = clientMovie.MovieId,
            ClientId = clientMovie.ClientId,
            ExpectedReturnDate = clientMovie.ExpectedReturnDate,
            StartDate = clientMovie.StartDate,
            ReturnDate = null
        };

        _context.Set<ClientMovie>().Add(newClientMovie);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return Created(clientMovie);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateClientMovieRequest clientMovie,
        CancellationToken cancellationToken)
    {
        if (IsInvalidModel(out var errors))
        {
            return BadRequest(errors); 
        }
        var existingClientMovie = await _context.Set<ClientMovie>().FindAsync([key], cancellationToken);
        if (existingClientMovie == null)
        {
            return NotFound();
        }

        existingClientMovie.StartDate = clientMovie.StartDate;
        existingClientMovie.ExpectedReturnDate = clientMovie.ExpectedReturnDate;

        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);
        return Updated(existingClientMovie);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key, CancellationToken cancellationToken)
    {
        var clientMovie = await _context.Set<ClientMovie>().FindAsync([key], cancellationToken);
        if (clientMovie == null)
        {
            return NotFound();
        }

        _context.Set<ClientMovie>().Remove(clientMovie);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}