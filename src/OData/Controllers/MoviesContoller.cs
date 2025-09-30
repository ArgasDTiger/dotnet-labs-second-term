using Microsoft.AspNetCore.OData.Results;

namespace OData.Controllers;

using EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

public sealed class MoviesController : ODataController
{
    private readonly MoviesRentContext _context;

    public MoviesController(MoviesRentContext context)
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

    public async Task<IActionResult> Post([FromBody] Movie movie)
    {
        _context.Set<Movie>().Add(movie);
        await _context.SaveChangesAsync();

        return Created(movie);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] Movie movie)
    {
         if (key != movie.Id)
         {
             return BadRequest("Key mismatch");
         }

         var existingMovie = await _context.Set<Movie>().FindAsync(key);
         if (existingMovie == null)
         {
             return NotFound();
         }

         existingMovie.Title = movie.Title;
         existingMovie.Description = movie.Description;
         existingMovie.CollateralValue = movie.CollateralValue;
         existingMovie.PricePerDay = movie.PricePerDay;

         try
         {
             await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
             if (!await MovieExists(key))
             {
                 return NotFound();
             }
             throw;
         }

         return Updated(existingMovie);
    }

    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Movie> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var movie = await _context.Set<Movie>().FindAsync(key);
        if (movie == null)
        {
            return NotFound();
        }

        delta.Patch(movie);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieExists(key))
            {
                return NotFound();
            }
            throw;
        }

        return Updated(movie);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        var movie = await _context.Set<Movie>().FindAsync(key);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Set<Movie>().Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieExists(Guid key)
    {
        return await _context.Set<Movie>().AnyAsync(m => m.Id == key);
    }
}