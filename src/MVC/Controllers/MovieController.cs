using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.ClientMovie;
using Shared.Requests.Movie;

namespace MVC.Controllers;

public sealed class MovieController : Controller
{
    private readonly IMovieRepository _movieRepository;

    public MovieController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var movies = await _movieRepository.GetAllMoviesAsync(cancellationToken);
        return View(movies);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        await _movieRepository.AddMovieAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _movieRepository.UpdateMovieAsync(id, request, cancellationToken);

        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Index)),
            _ => NotFound("Movie is not found")
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _movieRepository.DeleteMovieAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Index)),
            _ => NotFound("Movie is not found")
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rent([FromBody] RentMovieRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _movieRepository.RentMovieAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Index)),
            error => BadRequest(error.Message)
        );
    }
}