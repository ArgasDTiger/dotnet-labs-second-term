using EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using Shared.Responses;

namespace EntityFramework.Repositories;

public sealed class EfMovieRepository : IMovieRepository
{
    private readonly MoviesRentContext _context;

    public EfMovieRepository(MoviesRentContext context)
    {
        _context = context;
    }

    public Task<MovieResponse?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken)
    {
        return _context.Movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            CollateralValue = m.CollateralValue,
            PricePerDay = m.PricePerDay
        }).FirstOrDefaultAsync(m => m.Id == movieId, cancellationToken);
    }

    public Task<List<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        return _context.Movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            CollateralValue = m.CollateralValue,
            PricePerDay = m.PricePerDay
        }).ToListAsync(cancellationToken);
    }
}