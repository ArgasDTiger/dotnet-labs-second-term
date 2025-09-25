namespace Shared.Responses;

public sealed class ClientWithMoviesResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string MiddleName { get; init; }
    public required string LastName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string HomeAddress { get; init; }
    public required string? PassportSeries { get; init; }
    public required string PassportNumber { get; init; }
    public required IReadOnlyCollection<ClientMovieResponse>? RentedMovies { get; init; }
}