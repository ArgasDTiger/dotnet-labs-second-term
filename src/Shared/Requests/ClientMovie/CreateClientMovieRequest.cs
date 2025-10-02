using System.ComponentModel.DataAnnotations;
using Shared.Attributes;

namespace Shared.Requests.ClientMovie;

public sealed class CreateClientMovieRequest
{
    [Required]
    public Guid ClientId { get; init; }
    [Required]
    public Guid MovieId { get; init; }
    [Required]
    [NotEarlierThanUtcNow]
    public DateTime StartDate { get; init; }
    [Required]
    [NotEarlierThanUtcNow]
    public DateTime ExpectedReturnDate { get; init; }
}