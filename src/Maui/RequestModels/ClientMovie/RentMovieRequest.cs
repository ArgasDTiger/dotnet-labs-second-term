namespace Maui.RequestModels.ClientMovie;

public sealed record RentMovieRequest(
    Guid ClientId,
    Guid MovieId,
    DateTimeOffset StartDate);