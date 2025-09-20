namespace Shared.Exceptions;

public sealed class BelowMinimumLengthException : ArgumentException
{
    private const string ExceptionMessage = "Length of {0} should have at least {1} characters.";

    public BelowMinimumLengthException(string propertyName, int maxLength) : base(string.Format(ExceptionMessage,
        propertyName, maxLength))
    {
    }

    public static void ThrowIfBelowMaximumLength(string propertyName, int minLength, string value)
    {
        if (value.Length < minLength)
        {
            throw new BelowMinimumLengthException(propertyName, minLength);
        }
    }
}