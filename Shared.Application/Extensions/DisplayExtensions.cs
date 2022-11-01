namespace Shared.Application.Extensions;

public static class DisplayExtensions
{
    /// <summary>
    /// Display DateTime value with DateTimeFormat
    /// </summary>
    public static string ToString(this DateTime val, string format = Messages.DateTimeFormat, bool isTimeStamp = false)
    {
        if (format == Messages.DateTimeFormat && isTimeStamp)
            format = Messages.TimestampFormat;
        return val.ToString(format);
    }

    /// <summary>
    /// 
    /// </summary>
    public static string ToString(this DateTime? val, string format = Messages.DateTimeFormat, bool isTimeStamp = false, string NullValue = Messages.NullValue)
    { return (val.HasValue) ? val.Value.ToString(format, isTimeStamp) : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    public static string ToString(this long? val, string NullValue = Messages.NullValue)
    { return (val.HasValue) ? val.Value.ToString() : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    public static string Percentage(this decimal val)
    { return val.ToString("0.00") + "%"; }

    /// <summary>
    /// 
    /// </summary>
    public static string Percentage(this decimal? val, string NullValue = Messages.NullValue)
    { return val.HasValue ? val.Value.Percentage() : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    public static string Currency(this decimal val)
    { return val.ToString("0.00") + Messages.Currency; }

    /// <summary>
    /// 
    /// </summary>
    public static string Currency(this decimal? val, string NullValue = Messages.NullValue)
    { return val.HasValue ? val.Value.Currency() : NullValue; }
}
