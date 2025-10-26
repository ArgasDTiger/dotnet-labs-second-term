namespace Maui.RequestModels;

public sealed record CreateMovieRequest(
    string Title, 
    string Description,
    decimal CollateralValue,
    decimal PricePerDay);