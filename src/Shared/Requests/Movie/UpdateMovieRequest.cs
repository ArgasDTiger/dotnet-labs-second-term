using System.ComponentModel.DataAnnotations;
using static Shared.Constants.Entities.MovieConstants.Validation;

namespace Shared.Requests.Movie;

public sealed class UpdateMovieRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(TitleMaxLength)]
    public string Title { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [Range(0.0, double.MaxValue)]
    public decimal CollateralValue { get; init; }

    [Required(AllowEmptyStrings = false)]
    [Range(0.0, double.MaxValue)]
    public decimal PricePerDay { get; init; }
}