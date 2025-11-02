namespace Blazor.Core.Models;

public sealed record ClientMovie(
    Guid MovieId,
    string MovieTitle,
    DateTimeOffset StartDate,
    DateTimeOffset ExpectedReturnDate,
    DateTimeOffset? ReturnedDate,
    decimal PricePerDay);