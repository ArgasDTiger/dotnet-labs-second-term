namespace Shared.Responses;

public sealed class ClientMovieResponse
{
    public required Guid Id { get; init; }
    public required string MovieName { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime ExpectedReturnDate { get; init; }
}