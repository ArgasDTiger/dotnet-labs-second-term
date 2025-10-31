using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Requests.Client;
using Shared.Requests.ClientMovie;

namespace MVC.Controllers;

public sealed class ClientController : Controller
{
    private readonly IMovieRepository _movieRepository;
    private readonly IClientRepository _clientRepository;

    public ClientController(IMovieRepository movieRepository, IClientRepository clientRepository)
    {
        _movieRepository = movieRepository;
        _clientRepository = clientRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllClientsAsync(cancellationToken);
        return View(clients);
    }

    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(id, cancellationToken);

        if (client is null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index)); 
        }

        await _clientRepository.AddClientAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateClientRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _clientRepository.UpdateClientAsync(id, request, cancellationToken);

        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Details), new { id }),
            _ => NotFound("Client is not found")
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _clientRepository.DeleteClientAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Index)),
            _ => NotFound("Client is not found")
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnMovie(Guid movieId, Guid clientId, CancellationToken cancellationToken)
    {
        var request = new ReturnMovieRequest
        {
            ClientId = clientId,
            MovieId = movieId
        };
        var result = await _movieRepository.ReturnMovieAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            _ => RedirectToAction(nameof(Details), new { id = clientId }),
            error => BadRequest(error.Message)
        );
    }
}