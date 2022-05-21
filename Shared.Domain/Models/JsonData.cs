namespace Shared.Domain.Models;

/// <summary>
/// 
/// </summary>
public struct JsonDataValue
{
    /// <summary>
    /// 
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// 
/// </summary>
public struct JsonDataFieldValue
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
public struct JsonDataFieldOldNew
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
