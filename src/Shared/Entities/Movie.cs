namespace Shared.Entities;

public sealed class Movie
{
    public int Id { get; init; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal CollateralValue { get; set; }
    public decimal PricePerDay { get; set; }
    public ICollection<ClientMovie> ClientMovies { get; set; } = [];
}