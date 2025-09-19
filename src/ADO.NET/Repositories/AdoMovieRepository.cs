using System.Collections.Immutable;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Repositories;
using Shared.Responses;
using Shared.Settings;

namespace AdoNet.Repositories;

public sealed class AdoMovieRepository : IMovieRepository
{
    private readonly string _connectionString;

    public AdoMovieRepository(IOptions<ConnectionStringSettings> connectionStringSettings)
    {
        _connectionString = connectionStringSettings.Value.Default;
    }

    public async Task<MovieResponse?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(GetMovieByIdSql, connection);

        command.Parameters.Add("@MovieId", SqlDbType.Int).Value = movieId;

        await connection.OpenAsync(cancellationToken);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return GetMovieResponseFromReaderAsync(reader);
    }

    public async Task<ImmutableArray<MovieResponse>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(GetMoviesSql, connection);

        await connection.OpenAsync(cancellationToken);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var movies = ImmutableArray.CreateBuilder<MovieResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            movies.Add(GetMovieResponseFromReaderAsync(reader));
        }

        return movies.ToImmutable();
    }

    private static MovieResponse GetMovieResponseFromReaderAsync(SqlDataReader reader)
    {
        return new MovieResponse
        {
            Id = reader.GetGuid("Id"),
            Title = reader.GetString("Title"),
            Description = reader.GetString("Description"),
            CollateralValue = reader.GetDecimal("CollateralValue"),
            PricePerDay = reader.GetDecimal("PricePerDay")
        };
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