using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Repositories;
using Shared.Requests.ClientMovie;
using Shared.Requests.Movie;
using Shared.Responses;

namespace RazorPages.Pages.Movie;

public sealed class IndexModel : PageModel
{
    private readonly IMovieRepository _movieRepository;
    private readonly IClientRepository _clientRepository;

    public IndexModel(IMovieRepository movieRepository, IClientRepository clientRepository)
    {
        _movieRepository = movieRepository;
        _clientRepository = clientRepository;
    }

    public ImmutableArray<MovieResponse> Movies { get; private set; }
    public ImmutableArray<ClientResponse> Clients { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Movies = await _movieRepository.GetAllMoviesAsync(cancellationToken);
        Clients = await _clientRepository.GetAllClientsAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostRentAsync([FromBody] RentMovieRequest request, CancellationToken cancellationToken)
    {
        var result = await _movieRepository.RentMovieAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            _ => new OkResult(),
            error => new BadRequestObjectResult(error.Message)
        );
    }

    public async Task<IActionResult> OnPostCreateAsync([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movieId = await _movieRepository.AddMovieAsync(request, cancellationToken);
        return new OkObjectResult(movieId);
    }

    public async Task<IActionResult> OnPostUpdateAsync([FromQuery] Guid movieId, [FromBody] UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var result = await _movieRepository.UpdateMovieAsync(movieId, request, cancellationToken);

        return result.Match<IActionResult>(
            _ => new OkResult(),
            _ => new NotFoundObjectResult("Movie is not found")
        );
    }

    public async Task<IActionResult> OnPostDeleteAsync([FromQuery] Guid movieId, CancellationToken cancellationToken)
    {
        var result = await _movieRepository.DeleteMovieAsync(movieId, cancellationToken);
    
        return result.Match<IActionResult>(
            _ => new OkResult(), 
            _ => new NotFoundObjectResult("Movie is not found")
        );
    }
}