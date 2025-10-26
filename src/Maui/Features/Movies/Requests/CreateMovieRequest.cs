namespace Maui.Features.Movies.Requests;

public sealed record CreateMovieRequest(
    string Title, 
    string Description,
    decimal CollateralValue,
    decimal PricePerDay);