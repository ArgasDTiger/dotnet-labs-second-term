namespace Blazor.Core.Models;

public sealed record Movie
{
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal CollateralValue { get; set; }
    public decimal PricePerDay { get; set; }
};