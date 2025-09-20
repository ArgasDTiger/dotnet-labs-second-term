using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Settings;

namespace AdoNet.DatabaseProvider;

public sealed class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connectionString;
    private static readonly ConcurrentDictionary<string, ImmutableHashSet<PropertyInfo>> TypesCache = new();

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public DatabaseConnection(IOptions<ConnectionStringSettings> connectionStringSettings)
    {
        _connectionString = connectionStringSettings.Value.Default ??
                            throw new ArgumentNullException(nameof(connectionStringSettings));
    }

    public async IAsyncEnumerable<T> QueryAsync<T>(string query, object? parameters,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using SqlConnection sqlConnection = new(_connectionString);
        await using SqlCommand sqlCommand = new(query, sqlConnection);

        if (parameters is not null)
        {
            sqlCommand.Parameters.AddRange(parameters.GetType().GetProperties());
        }

        await sqlConnection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return MapToEntity<T>(reader);
        }
    }

    public IAsyncEnumerable<T> QueryAsync<T>(string query, CancellationToken cancellationToken) =>
        QueryAsync<T>(query, null, cancellationToken);

    public async Task<T?> QuerySingleAsync<T>(string query, object? parameters, CancellationToken cancellationToken)

    {
        await using SqlConnection sqlConnection = new(_connectionString);
        await using SqlCommand sqlCommand = new(query, sqlConnection);

        if (parameters is not null)
        {
            sqlCommand.Parameters.AddRange(parameters.GetType().GetProperties()
                .Select(p => new SqlParameter(p.Name, p.GetValue(parameters))).ToArray());
        }

        await sqlConnection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken)) return default;

        return MapToEntity<T>(reader);
    }

    public Task<T?> QuerySingleAsync<T>(string query, CancellationToken cancellationToken) =>
        QuerySingleAsync<T>(query, null, cancellationToken);

    private static T MapToEntity<T>(SqlDataReader reader)
    {
        T entity = Activator.CreateInstance<T>();
        Type entityType = typeof(T);
        ImmutableHashSet<PropertyInfo> entityProperties = GetWritableTypeProperties(entityType);
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            PropertyInfo? correspondingEntityProperty =
                entityProperties.FirstOrDefault(p => p.Name.Equals(columnName));

            if (correspondingEntityProperty is null) continue;

            if (reader.IsDBNull(i))
            {
                if (!correspondingEntityProperty.IsNullable)
                {
                    throw new InvalidOperationException(
                        $"The property {correspondingEntityProperty.Name} of type {entityType.FullName} is not nullable.");
                }

                entityType.GetProperty(correspondingEntityProperty.Name)!.SetValue(entity, default);
            }
            else
            {
                entityType.GetProperty(correspondingEntityProperty.Name)!.SetValue(entity, reader[i]);
            }
        }

        return entity;
    }

    private static ImmutableHashSet<PropertyInfo> GetWritableTypeProperties(Type type)
    {
        if (type.FullName is null)
        {
            return
                [..type.GetProperties().Where(p => p.CanWrite).Select(p => new PropertyInfo(p.Name, p.PropertyType))];
        }

        bool typeCached = TypesCache.TryGetValue(type.FullName, out var properties);
        if (typeCached && properties is not null)
        {
            return properties;
        }

        properties =
            [..type.GetProperties().Where(p => p.CanWrite).Select(p => new PropertyInfo(p.Name, p.PropertyType))];
        TypesCache.TryAdd(type.FullName, properties);
        return properties;
    }

    private record PropertyInfo(string Name, Type Type)
    {
        public bool IsNullable => !Type.IsValueType || Nullable.GetUnderlyingType(Type) != null;
    }
}