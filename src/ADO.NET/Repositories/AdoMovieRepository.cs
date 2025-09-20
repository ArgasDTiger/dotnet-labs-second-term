using System.Collections.Immutable;
using AdoNet.DatabaseProvider;
using Shared.Extensions;
using Shared.Repositories;
using Shared.Responses;

namespace AdoNet.Repositories;

public sealed class AdoMovieRepository : IMovieRepository
{
    private readonly IDatabaseConnection _databaseConnection;

    public AdoMovieRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public async Task<MovieResponse?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return await _databaseConnection.QuerySingleAsync<MovieResponse?>(
            GetMovieByIdSql,
            parameters: new { MovieId = movieId },
            cancellationToken);
    }

    public async Task<ImmutableArray<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        return await _databaseConnection.QueryAsync<MovieResponse>(GetMoviesSql, cancellationToken).ToImmutableArrayAsync(cancellationToken);
    }

    #region SQL queries

    private const string GetMovieByIdSql = """
                                               SELECT Id, Title, Description, CollateralValue, PricePerDay 
                                               FROM Movies 
                                               WHERE Id = @MovieId
                                           """;

    private const string GetMoviesSql = """
                                            SELECT Id, Title, Description, CollateralValue, PricePerDay 
                                            FROM Movies 
                                        """;

    #endregion
}