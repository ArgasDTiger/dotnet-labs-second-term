using System.Collections.Immutable;
using EntityFramework.Data;
using EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
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

    public Task<MovieResponse?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return _context.Set<Movie>().Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            CollateralValue = m.CollateralValue,
            PricePerDay = m.PricePerDay
        }).FirstOrDefaultAsync(m => m.Id == movieId, cancellationToken);
    }

    public Task<ImmutableArray<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        return _context.Set<Movie>().Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            CollateralValue = m.CollateralValue,
            PricePerDay = m.PricePerDay
        }).ToImmutableArrayAsync(cancellationToken);
    }
}