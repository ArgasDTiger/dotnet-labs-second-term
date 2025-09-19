namespace Shared.Entities;

public sealed class Client
{
    public Guid Id { get; init; }
    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string HomeAddress { get; set; } = null!;
    public string? PassportSeries { get; set; }
    public string PassportNumber { get; set; } = null!;
    public ICollection<ClientMovie> ClientMovies { get; set; } = [];
}