using System.Collections.Immutable;

namespace Maui.Features.Clients.Models;

public sealed record Client(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string PhoneNumber,
    string HomeAddress,
    string? PassportSeries,
    string PassportNumber,
    ImmutableArray<ClientMovie>? RentedMovies);