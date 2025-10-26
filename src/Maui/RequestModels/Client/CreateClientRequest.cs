namespace Maui.RequestModels.Client;

public sealed record CreateClientRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    string PhoneNumber,
    string HomeAddress,
    string? PassportSeries,
    string PassportNumber);