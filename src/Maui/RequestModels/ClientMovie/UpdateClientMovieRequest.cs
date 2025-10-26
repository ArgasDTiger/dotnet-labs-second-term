namespace Maui.RequestModels.ClientMovie;

public sealed record UpdateClientMovieRequest(
    DateTimeOffset StartDate, 
    DateTimeOffset ExpectedReturnDate);