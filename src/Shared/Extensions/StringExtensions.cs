namespace Shared.Extensions;

public static class StringExtension
{
    public static string ToCamelCaseWithUnderscore(this string str)
    {                    
        if(!string.IsNullOrEmpty(str) && str.Length > 1)
        {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
        return "_" + str.ToLowerInvariant();
    }
}