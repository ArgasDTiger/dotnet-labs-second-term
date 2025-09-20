namespace AdoNet.DatabaseProvider;

public interface IDatabaseConnection
{
    IAsyncEnumerable<T> QueryAsync<T>(string query, object? parameters, CancellationToken cancellationToken);
    IAsyncEnumerable<T> QueryAsync<T>(string query, CancellationToken cancellationToken);
    Task<T?> QuerySingleAsync<T>(string query, object? parameters, CancellationToken cancellationToken);
    Task<T?> QuerySingleAsync<T>(string query, CancellationToken cancellationToken);
}