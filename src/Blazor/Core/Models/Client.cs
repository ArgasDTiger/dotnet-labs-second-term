namespace Blazor.Core.Models;

public sealed record Client
{
    public Guid Id { get; init; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string HomeAddress { get; set; } = string.Empty;
    public string? PassportSeries { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public ImmutableArray<ClientMovie> RentedMovies { get; set; } = [];
}