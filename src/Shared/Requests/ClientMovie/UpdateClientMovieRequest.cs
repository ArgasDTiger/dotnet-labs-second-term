using System.ComponentModel.DataAnnotations;
using Shared.Attributes;

namespace Shared.Requests.ClientMovie;

public sealed class UpdateClientMovieRequest
{
    [Required]
    [NotEarlierThanUtcNow]
    public DateTime StartDate { get; init; }
    [Required]
    [NotEarlierThanUtcNow]
    public DateTime ExpectedReturnDate { get; init; }
}