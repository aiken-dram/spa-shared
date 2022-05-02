namespace Shared.Domain.Models;

/// <summary>
/// 
/// </summary>
public struct JsonEventDataValue
{
    /// <summary>
    /// 
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// 
/// </summary>
public struct JsonEventDataFieldValue
{
    /// <summary>
    /// 
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// 
/// </summary>
public struct JsonEventDataFieldOldNew
{
    /// <summary>
    /// 
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Old { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string New { get; set; }
}
