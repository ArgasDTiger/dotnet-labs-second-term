namespace Maui.Features.Clients.Requests;

public sealed record UpdateClientMovieRequest(
    DateTimeOffset StartDate, 
    DateTimeOffset ExpectedReturnDate);