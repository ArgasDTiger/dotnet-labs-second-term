using Microsoft.AspNetCore.OData.Results;
using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Shared.Entities;
using Shared.Requests.Movie;

namespace OData.Controllers;

public sealed class MoviesController : BaseController
{
    private readonly MoviesRentContext _context;

    public MoviesController(MoviesRentContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Movie> Get()
    {
        return _context.Set<Movie>();
    }

    [EnableQuery]
    public SingleResult<Movie> Get([FromRoute] Guid key)
    {
        return SingleResult.Create(_context.Set<Movie>().Where(m => m.Id == key));
    }

    public async Task<IActionResult> Post([FromBody] CreateMovieRequest movie, CancellationToken cancellationToken)
    {
        Movie newMovie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = movie.Title,
            Description = movie.Description,
            CollateralValue = movie.CollateralValue,
            PricePerDay = movie.PricePerDay
        };

        _context.Set<Movie>().Add(newMovie);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return Created(movie);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateMovieRequest movie,
        CancellationToken cancellationToken)
    {
        var existingMovie = await _context.Set<Movie>().FindAsync([key], cancellationToken);
        if (existingMovie == null)
        {
            return NotFound();
        }

        existingMovie.Title = movie.Title;
        existingMovie.Description = movie.Description;
        existingMovie.CollateralValue = movie.CollateralValue;
        existingMovie.PricePerDay = movie.PricePerDay;

        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);
        return Updated(existingMovie);
    }

    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Movie> delta,
        CancellationToken cancellationToken)
    {
        var movie = await _context.Set<Movie>().FindAsync([key], cancellationToken);
        if (movie == null)
        {
            return NotFound();
        }

        delta.Patch(movie);

        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);
        return Updated(movie);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key, CancellationToken cancellationToken)
    {
        var movie = await _context.Set<Movie>().FindAsync([key], cancellationToken);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Set<Movie>().Remove(movie);
        bool saved = await SaveChangesAsync(cancellationToken);
        if (!saved) return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}