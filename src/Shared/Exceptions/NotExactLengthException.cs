namespace Shared.Exceptions;

public sealed class NotExactLengthException : ArgumentException
{
    private const string ExceptionMessage = "Length of {0} should be exactly {1} characters.";

    public NotExactLengthException(string propertyName, int maxLength) : base(string.Format(ExceptionMessage,
        propertyName, maxLength))
    {
    }

    public static void ThrowIfNotExactLength(string propertyName, int length, string value)
    {
        if (value.Length != length)
        {
            throw new NotExactLengthException(propertyName, length);
        }
    }
}