using System.Collections.Immutable;
using EntityFramework.Data;
using EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Shared.Entities;
using Shared.Models;
using Shared.Repositories;
using Shared.Requests.ClientMovie;
using Shared.Requests.Movie;
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

    public async Task<Guid> AddMovieAsync(CreateMovieRequest movie, CancellationToken cancellationToken)
    {
        Movie newMovie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = movie.Title,
            Description = movie.Description,
            CollateralValue = movie.CollateralValue,
            PricePerDay = movie.PricePerDay
        };

        _context.Set<Movie>().Add(newMovie);
        return await _context.SaveChangesAsync(cancellationToken) > 0
            ? newMovie.Id
            : throw new Exception("Failed to add movie.");
    }

    public async Task<OneOf<None, NotFound>> UpdateMovieAsync(Guid movieId, UpdateMovieRequest client,
        CancellationToken cancellationToken)
    {
        Movie? movieToUpdate = await _context.Set<Movie>().FindAsync([movieId], cancellationToken);
        if (movieToUpdate is null)
        {
            return new NotFound();
        }

        movieToUpdate.Title = client.Title;
        movieToUpdate.Description = client.Description;
        movieToUpdate.CollateralValue = client.CollateralValue;
        movieToUpdate.PricePerDay = client.PricePerDay;
        
        await _context.SaveChangesAsync(cancellationToken);
        return new None();
    }

    public async Task<OneOf<None, NotFound>> DeleteMovieAsync(Guid clientId, CancellationToken cancellationToken)
    {
        bool movieExists = await _context.Set<Movie>().AnyAsync(m => m.Id == clientId, cancellationToken);
        if (!movieExists)
        {
            return new NotFound();
        }

        int result = await _context.Set<Movie>().Where(m => m.Id == clientId).ExecuteDeleteAsync(cancellationToken);
        return result > 0 ? new None() : throw new Exception("Failed to delete movie.");
    }

    public async Task<OneOf<None, ErrorMessage>> RentMovieAsync(RentMovieRequest request,
        CancellationToken cancellationToken)
    {
        bool canRent = await CanClientRentMovie(request, cancellationToken);
        if (!canRent)
        {
            return new ErrorMessage("Client cannot rent this movie.");
        }

        ClientMovie newClientMovie = new ClientMovie
        {
            ClientId = request.ClientId,
            MovieId = request.MovieId,
            ExpectedReturnDate = request.ExpectedReturnDate,
            StartDate = DateTime.UtcNow
        };

        _context.Set<ClientMovie>().Add(newClientMovie);
        return await _context.SaveChangesAsync(cancellationToken) > 0
            ? new None()
            : throw new Exception("Failed to rent movie.");
    }

    public async Task<OneOf<None, ErrorMessage>> ReturnMovieAsync(ReturnMovieRequest request,
        CancellationToken cancellationToken)
    {
        ClientMovie? rentedMovie = await _context.Set<ClientMovie>()
            .FirstOrDefaultAsync(cm =>
                    cm.ClientId == request.ClientId && cm.MovieId == request.MovieId && cm.ReturnDate != null,
                cancellationToken);
        if (rentedMovie is null)
        {
            return new ErrorMessage("Movie is not rented.");
        }
        
        rentedMovie.ReturnDate = DateTime.UtcNow;
        return await _context.SaveChangesAsync(cancellationToken) > 0
            ? new None()
            : throw new Exception("Failed to return movie.");
    }

    private async Task<bool> CanClientRentMovie(RentMovieRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _context.Set<Client>()
            .Where(c => c.Id == request.ClientId)
            .Select(c => new
            {
                ClientExists = true,
                MovieExists = _context.Set<Movie>().Any(m => m.Id == request.MovieId),
                HasOverlap = _context.Set<ClientMovie>().Any(cm =>
                    cm.ClientId == request.ClientId
                    && cm.MovieId == request.MovieId
                    && cm.ReturnDate != null)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return validationResult != null
               && validationResult.MovieExists
               && !validationResult.HasOverlap;
    }
}