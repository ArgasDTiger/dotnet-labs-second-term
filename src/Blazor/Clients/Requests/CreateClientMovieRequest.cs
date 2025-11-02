namespace Maui.Features.Clients.Requests;

public sealed record CreateClientMovieRequest(
    Guid ClientId,
    Guid MovieId,
    DateTimeOffset StartDate,
    DateTimeOffset ExpectedReturnDate);