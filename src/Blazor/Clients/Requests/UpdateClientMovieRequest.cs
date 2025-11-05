namespace Blazor.Clients.Requests;

public sealed record UpdateClientMovieRequest(
    DateTimeOffset StartDate, 
    DateTimeOffset ExpectedReturnDate);