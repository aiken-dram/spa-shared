namespace Shared.Domain.Attributes;

/// <summary>
/// Indicates that the property will be audited
/// </summary>
public class AuditAttribute : Attribute
{
    /// <summary>
    /// Name of dictionary for storing audit data from
    /// </summary>
    public string? Dictionary { get; private set; }

    /// <summary>
    /// Include time in displaying DateTime value
    /// </summary>
    public bool IsTimeStamp { get; private set; } = false;

    /// <summary>
    /// Hide value when auditing this property
    /// </summary>
    public bool HideValue { get; private set; } = false;

    /// <summary>
    /// Format decimal value as currency
    /// </summary>
    public bool IsCurreny { get; private set; } = false;

    /// <summary>
    /// Format string value as char boolean
    /// </summary>
    public bool IsCharBoolean { get; private set; } = false;

    /// <summary>
    /// Default constructor
    /// </summary>
    public AuditAttribute()
    {

    }

    /// <summary>
    /// Constructor with dictionary
    /// </summary>
    /// <param name="dictionary">Name of dictionary</param>
    /// <param name="hideValue">Hide value from audit</param>
    /// <param name="isTimeStamp">Include time in displaying DateTime value</param>
    /// <param name="isCurreny">Format value as currency</param>
    /// <param name="isCharBoolean">Format value as char boolean</param>
    public AuditAttribute(
        string? dictionary = null,
        bool hideValue = false,
        bool isTimeStamp = false,
        bool isCurreny = false,
        bool isCharBoolean = false)
    {
        Dictionary = dictionary;
        HideValue = hideValue;
        IsTimeStamp = isTimeStamp;
        IsCharBoolean = isCharBoolean;
        IsCurreny = isCurreny;
    }
}

/// <summary>
/// Indicates that entity audit will be handled from ChangeTracker.Entries tracker
/// </summary>
public class AutoAuditAttribute : Attribute
{

}
