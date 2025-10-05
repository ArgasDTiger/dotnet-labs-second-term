using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Repositories;
using Shared.Requests.ClientMovie;
using Shared.Responses;

namespace RazorPages.Pages.Client;

public sealed class Details : PageModel
{
    private readonly IClientRepository _clientRepository;
    private readonly IMovieRepository _movieRepository;

    public Details(IClientRepository clientRepository, IMovieRepository movieRepository)
    {
        _clientRepository = clientRepository;
        _movieRepository = movieRepository;
    }

    public ClientWithMoviesResponse? Client { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        Client = await _clientRepository.GetClientByIdAsync(id, cancellationToken);

        if (Client is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostReturnMovieAsync([FromBody] ReturnMovieRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _movieRepository.ReturnMovieAsync(request, cancellationToken);
        return result.Match<IActionResult>(
            _ => new OkResult(),
            error => new BadRequestObjectResult(error.Message)
        );
    }
}