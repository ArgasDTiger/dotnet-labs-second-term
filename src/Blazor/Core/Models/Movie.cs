namespace Blazor.Core.Models;

public sealed record Movie(
    Guid Id,
    string Title,
    string Description,
    decimal CollateralValue,
    decimal PricePerDay);