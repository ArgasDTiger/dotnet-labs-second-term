namespace Maui.RequestModels.ClientMovie;

public sealed record CreateClientMovieRequest(
    Guid ClientId,
    Guid MovieId,
    DateTimeOffset StartDate,
    DateTimeOffset ExpectedReturnDate);