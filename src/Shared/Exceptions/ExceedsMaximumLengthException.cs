namespace Shared.Exceptions;

public sealed class ExceedsMaximumLengthException : ArgumentException
{
    private const string ExceptionMessage = "Length of {0} shouldn't exceed {1} characters.";

    public ExceedsMaximumLengthException(string propertyName, int maxLength) : base(string.Format(ExceptionMessage,
        propertyName, maxLength))
    {
    }

    public static void ThrowIfExceedsMaximumLength(string propertyName, int maxLength, string value)
    {
        if (value.Length > maxLength)
        {
            throw new ExceedsMaximumLengthException(propertyName, maxLength);
        }
    }
}