using Shared.Exceptions;

namespace Shared.Entities;

public sealed class Client
{
    private string _firstName = null!;
    private string _middleName = null!;
    private string _lastName = null!;
    private string _phoneNumber = null!;
    private string _homeAddress = null!;
    private string? _passportSeries;
    private string _passportNumber = null!;
    private ICollection<ClientMovie> _clientMovies = [];
    
    public const int FirstNameMaxLength = 50;
    public const int MiddleNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int PhoneNumberMaxLength = 15;
    public const int HomeAddressMaxLength = 255;
    public const int PassportSeriesMaxLength = 2;
    public const int PassportNumberMaxLength = 9;

    public Guid Id { get; init; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(FirstName), FirstNameMaxLength, value);
            _firstName = value;
        }
    }

    public string MiddleName
    {
        get => _middleName;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(MiddleName), MiddleNameMaxLength, value);
            _middleName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(LastName), LastNameMaxLength, value);   
            _lastName = value;
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            BelowMinimumLengthException.ThrowIfBelowMaximumLength(nameof(PhoneNumber), 10, value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(PhoneNumber), PhoneNumberMaxLength, value);
            _phoneNumber = value;
        }
    }

    public string HomeAddress
    {
        get => _homeAddress;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(HomeAddress), HomeAddressMaxLength, value);
            _homeAddress = value;
        }
    }

    public string? PassportSeries
    {
        get => _passportSeries;
        set
        {
            if (value is not null)
            {
                NotExactLengthException.ThrowIfNotExactLength(nameof(PassportSeries), PassportSeriesMaxLength, value);
            }
            _passportSeries = value;
        }
    }

    public string PassportNumber
    {
        get => _passportNumber;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            ExceedsMaximumLengthException.ThrowIfExceedsMaximumLength(nameof(PassportSeries), PassportNumberMaxLength, value);
            _passportNumber = value;
        }
    }

    public ICollection<ClientMovie> ClientMovies
    {
        get => _clientMovies;
        set => _clientMovies = value ?? throw new ArgumentNullException(nameof(value));
    }
}