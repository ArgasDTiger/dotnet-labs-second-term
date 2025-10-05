using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Repositories;
using Shared.Requests.Client;
using Shared.Responses;

namespace RazorPages.Pages.Client;

public sealed class IndexModel : PageModel
{
    private readonly IMovieRepository _movieRepository;
    private readonly IClientRepository _clientRepository;

    public IndexModel(IMovieRepository movieRepository, IClientRepository clientRepository)
    {
        _movieRepository = movieRepository;
        _clientRepository = clientRepository;
    }

    public ImmutableArray<ClientResponse> Clients { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Clients = await _clientRepository.GetAllClientsAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientId = await _clientRepository.AddClientAsync(request, cancellationToken);
        return new OkObjectResult(clientId);
    }

    public async Task<IActionResult> OnPostUpdateAsync([FromQuery] Guid clientId, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var result = await _clientRepository.UpdateClientAsync(clientId, request, cancellationToken);

        return result.Match<IActionResult>(
            _ => new OkResult(),
            _ => new NotFoundObjectResult("Client is not found")
        );
    }

    public async Task<IActionResult> OnPostDeleteAsync([FromQuery] Guid clientId, CancellationToken cancellationToken)
    {
        var result = await _clientRepository.DeleteClientAsync(clientId, cancellationToken);
    
        return result.Match<IActionResult>(
            _ => new OkResult(), 
            _ => new NotFoundObjectResult("Client is not found")
        );
    }
}