namespace Blazor.Clients.Requests;

public sealed record UpdateClientRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    string PhoneNumber,
    string HomeAddress,
    string? PassportSeries,
    string PassportNumber);