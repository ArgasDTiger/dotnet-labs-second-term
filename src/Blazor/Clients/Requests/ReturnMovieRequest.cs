namespace Blazor.Clients.Requests;

public sealed record ReturnMovieRequest(Guid ClientId, Guid MovieId);