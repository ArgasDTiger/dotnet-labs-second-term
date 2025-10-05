namespace Shared.Requests.ClientMovie;

public sealed class RentMovieRequest
{
    public Guid ClientId { get; init; }
    public Guid MovieId { get; init; }
    public DateTime ExpectedReturnDate { get; init; }
}