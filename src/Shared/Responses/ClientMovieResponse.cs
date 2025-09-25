namespace Shared.Responses;

public sealed class ClientMovieResponse
{
    public required string MovieTitle { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime ExpectedReturnDate { get; init; }
    public required decimal PricePerDay { get; init; }
}