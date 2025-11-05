namespace Blazor.Clients.Requests;

public sealed record RentMovieRequest(
    Guid ClientId,
    Guid MovieId,
    DateTimeOffset StartDate);