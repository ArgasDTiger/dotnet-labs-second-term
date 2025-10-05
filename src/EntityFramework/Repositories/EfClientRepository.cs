using System.Collections.Immutable;
using EntityFramework.Data;
using EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Repositories;
using Shared.Requests.Client;
using Shared.Responses;
using OneOf;
using OneOf.Types;

namespace EntityFramework.Repositories;

public sealed class EfClientRepository : IClientRepository
{
    private readonly MoviesRentContext _context;

    public EfClientRepository(MoviesRentContext context)
    {
        _context = context;
    }

    public Task<ClientWithMoviesResponse?> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return _context.Set<Client>().Select(c => new ClientWithMoviesResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            MiddleName = c.MiddleName,
            LastName = c.LastName,
            PhoneNumber = c.PhoneNumber,
            HomeAddress = c.HomeAddress,
            PassportSeries = c.PassportSeries,
            PassportNumber = c.PassportNumber,
            RentedMovies = c.ClientMovies.Select(cm => new ClientMovieResponse
            {
                MovieTitle = cm.Movie.Title,
                StartDate = cm.StartDate,
                ExpectedReturnDate = cm.ExpectedReturnDate,
                ReturnedDate = cm.ReturnDate,
                PricePerDay = cm.Movie.PricePerDay,
                MovieId = cm.MovieId
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);
    }

    public Task<ImmutableArray<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        return _context.Set<Client>().Select(c => new ClientResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            MiddleName = c.MiddleName,
            LastName = c.LastName,
            PhoneNumber = c.PhoneNumber,
            HomeAddress = c.HomeAddress,
            PassportSeries = c.PassportSeries,
            PassportNumber = c.PassportNumber,
        }).ToImmutableArrayAsync(cancellationToken);
    }

    public async Task<Guid> AddClientAsync(CreateClientRequest client,
        CancellationToken cancellationToken)
    {
        Client newClient = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = client.FirstName,
            MiddleName = client.MiddleName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber,
            HomeAddress = client.HomeAddress,
            PassportSeries = client.PassportSeries,
            PassportNumber = client.PassportNumber
        };

        _context.Set<Client>().Add(newClient);
        return await _context.SaveChangesAsync(cancellationToken) > 0
            ? newClient.Id
            : throw new Exception("Failed to add client.");
    }

    public async Task<OneOf<None, NotFound>> UpdateClientAsync(Guid clientId, UpdateClientRequest client,
        CancellationToken cancellationToken)
    {
        Client? clientToUpdate = await _context.Set<Client>().FindAsync([clientId], cancellationToken);
        if (clientToUpdate is null)
        {
            return new NotFound();
        }

        clientToUpdate.FirstName = client.FirstName;
        clientToUpdate.MiddleName = client.MiddleName;
        clientToUpdate.LastName = client.LastName;
        clientToUpdate.PhoneNumber = client.PhoneNumber;
        clientToUpdate.HomeAddress = client.HomeAddress;
        clientToUpdate.PassportSeries = client.PassportSeries;
        clientToUpdate.PassportNumber = client.PassportNumber;
        return await _context.SaveChangesAsync(cancellationToken) > 0
            ? new None()
            : throw new Exception("Failed to update client.");
    }

    public async Task<OneOf<None, NotFound>> DeleteClientAsync(Guid clientId,
        CancellationToken cancellationToken)
    {
        bool clientExists = await _context.Set<Client>().AnyAsync(c => c.Id == clientId, cancellationToken);
        if (!clientExists)
        {
            return new NotFound();
        }

        int result = await _context.Set<Client>().Where(c => c.Id == clientId).ExecuteDeleteAsync(cancellationToken);
        return result > 0 ? new None() : throw new Exception("Failed to delete client.");
    }
}