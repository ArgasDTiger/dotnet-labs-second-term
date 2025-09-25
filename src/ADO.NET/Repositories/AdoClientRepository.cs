using System.Collections.Immutable;
using System.Data;
using AdoNet.DatabaseProvider;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Extensions;
using Shared.Repositories;
using Shared.Responses;
using Shared.Settings;

namespace AdoNet.Repositories;

public sealed class AdoClientRepository : IClientRepository
{
    private readonly IDatabaseConnection _databaseConnection;
    private readonly string _connectionString;

    public AdoClientRepository(IOptions<ConnectionStringSettings> connectionStringSettings,
        IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
        _connectionString = connectionStringSettings.Value.Default;
    }

    public async Task<ClientWithMoviesResponse?> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(GetClientByIdSql, connection);

        command.Parameters.Add("@ClientId", SqlDbType.UniqueIdentifier).Value = clientId;

        await connection.OpenAsync(cancellationToken);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        ClientWithMoviesResponse? client = null;
        var movies = new List<ClientMovieResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            client ??= new ClientWithMoviesResponse
            {
                Id = reader.GetGuid("Id"),
                FirstName = reader.GetString("FirstName"),
                MiddleName = reader.GetString("MiddleName"),
                LastName = reader.GetString("LastName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                HomeAddress = reader.GetString("HomeAddress"),
                PassportSeries = await reader.IsDBNullAsync("PassportSeries", cancellationToken)
                    ? null
                    : reader.GetString("PassportSeries"),
                PassportNumber = reader.GetString("PassportNumber"),
                RentedMovies = movies
            };

            if (!await reader.IsDBNullAsync("MovieTitle", cancellationToken))
            {
                movies.Add(new ClientMovieResponse
                {
                    MovieTitle = reader.GetString("MovieTitle"),
                    StartDate = reader.GetDateTime("StartDate"),
                    ExpectedReturnDate = reader.GetDateTime("ExpectedReturnDate"),
                    PricePerDay = reader.GetDecimal("PricePerDay"),
                });
            }
        }

        return client;
    }

    public async Task<ImmutableArray<ClientResponse>> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        return await _databaseConnection.QueryAsync<ClientResponse>(GetClientsSql, cancellationToken)
            .ToImmutableArrayAsync(cancellationToken);
    }

    #region SQL queries

    private const string GetClientByIdSql = """
                                            SELECT 
                                                c.Id, c.FirstName, c.MiddleName, c.LastName, c.PhoneNumber, 
                                                c.HomeAddress, c.PassportSeries, c.PassportNumber,
                                                cm.StartDate, cm.ExpectedReturnDate,
                                                m.Title as MovieTitle, m.PricePerDay
                                            FROM Clients c
                                            LEFT JOIN ClientMovie cm ON c.Id = cm.ClientId
                                            LEFT JOIN Movies m ON cm.MovieId = m.Id
                                            WHERE c.Id = @ClientId
                                            """;

    private const string GetClientsSql = """
                                             SELECT Id, FirstName, MiddleName, LastName, PhoneNumber, HomeAddress, PassportSeries, PassportNumber
                                             FROM Clients
                                         """;

    #endregion
}