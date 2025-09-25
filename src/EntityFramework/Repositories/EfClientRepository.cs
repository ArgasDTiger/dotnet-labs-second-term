using System.Collections.Immutable;
using EntityFramework.Data;
using EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Repositories;
using Shared.Responses;

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
                PricePerDay = cm.Movie.PricePerDay
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
}