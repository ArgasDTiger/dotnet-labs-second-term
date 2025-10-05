namespace Shared.Requests.ClientMovie;

public sealed class ReturnMovieRequest
{
    public Guid ClientId { get; init; }
    public Guid MovieId { get; init; }   
}