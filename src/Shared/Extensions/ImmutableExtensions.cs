using System.Collections.Immutable;

namespace Shared.Extensions;

public static class ImmutableExtensions
{
    public static async Task<ImmutableArray<TSource>> ToImmutableArrayAsync<TSource>(
        this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<TSource>();
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            builder.Add(element);
        }

        return builder.ToImmutable();
    }
}