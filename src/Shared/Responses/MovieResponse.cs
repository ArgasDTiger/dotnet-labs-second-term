namespace Shared.Responses;

public sealed class MovieResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required decimal CollateralValue { get; init; }
    public required decimal PricePerDay { get; init; }
}