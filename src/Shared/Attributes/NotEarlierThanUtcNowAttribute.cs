using System.ComponentModel.DataAnnotations;

namespace Shared.Attributes;

public sealed class NotEarlierThanUtcNowAttribute : ValidationAttribute
{
    public NotEarlierThanUtcNowAttribute()
    {
        ErrorMessage = "The date and time cannot be earlier than the current UTC time.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case DateTime dateTime:
            {
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc); 
                }

                if (dateTime < DateTime.UtcNow)
                {
                    if (validationContext.MemberName != null)
                        return new ValidationResult(ErrorMessage, [validationContext.MemberName]);
                }

                break;
            }
            case DateTimeOffset dateTimeOffset:
            {
                if (dateTimeOffset < DateTimeOffset.UtcNow)
                {
                    if (validationContext.MemberName != null)
                        return new ValidationResult(ErrorMessage, [validationContext.MemberName]);
                }

                break;
            }
            default:
            {
                if (value != null) 
                {
                    throw new InvalidOperationException("NotEarlierThanUtcNowAttribute can only be applied to DateTime or DateTimeOffset properties.");
                }

                break;
            }
        }

        return ValidationResult.Success;
    }
}