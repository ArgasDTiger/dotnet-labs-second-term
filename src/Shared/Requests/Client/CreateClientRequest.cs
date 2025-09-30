using System.ComponentModel.DataAnnotations;
using OneOf.Types;
using static Shared.Constants.Entities.ClientConstants.Validation;

namespace Shared.Requests.Client;

public sealed class CreateClientRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(MiddleNameMaxLength)]
    public string MiddleName { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(HomeAddressMaxLength)]
    public string HomeAddress { get; init; } = null!;

    [Length(PassportSeriesLength, PassportSeriesLength)]
    public string? PassportSeries { get; init; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(PassportNumberMaxLength)]
    public string PassportNumber { get; init; } = null!;
}