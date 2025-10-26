namespace Maui.RequestModels;

public sealed record UpdateMovieRequest(
    string Title, 
    string Description,
    decimal CollateralValue,
    decimal PricePerDay);