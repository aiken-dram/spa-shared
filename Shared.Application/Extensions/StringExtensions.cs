namespace Shared.Application.Extensions;

/// <summary>
/// Extensions for string
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates string to maxLength
    /// </summary>
    /// <param name="value">original string</param>
    /// <param name="maxLength">maximum length</param>
    /// <returns>truncated string</returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}
