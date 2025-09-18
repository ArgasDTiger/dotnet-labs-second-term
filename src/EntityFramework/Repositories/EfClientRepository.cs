using EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
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

    public Task<ClientWithMoviesResponse?> GetClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return _context.Clients.Select(c => new ClientWithMoviesResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            MiddleName = c.MiddleName,
            LastName = c.LastName,
            PhoneNumber = c.PhoneNumber,
            HomeAddress = c.HomeAddress,
            PassportSeries = c.PassportSeries,
            PassportNumber = c.PassportNumber,
            RentedMovies = c.ClientMovies.Select(cm => new MovieResponse
            {
                Id = cm.MovieId,
                Title = cm.Movie.Title,
                Description = cm.Movie.Description,
                CollateralValue = cm.Movie.CollateralValue,
                PricePerDay = cm.Movie.PricePerDay,
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);
    }

    public Task<List<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        return _context.Clients.Select(c => new ClientResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            MiddleName = c.MiddleName,
            LastName = c.LastName,
            PhoneNumber = c.PhoneNumber,
            HomeAddress = c.HomeAddress,
            PassportSeries = c.PassportSeries,
            PassportNumber = c.PassportNumber,
        }).ToListAsync(cancellationToken);
    }
}