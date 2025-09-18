namespace Shared.Responses;

public sealed class ClientResponse
{
    public required int Id { get; init; }
    public required string FirstName { get; init; }
    public required string MiddleName { get; init; }
    public required string LastName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string HomeAddress { get; init; }
    public required string? PassportSeries { get; init; }
    public required string PassportNumber { get; init; }
}