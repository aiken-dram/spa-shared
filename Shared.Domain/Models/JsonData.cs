namespace Shared.Domain.Models;

/// <summary>
/// JSON for value
/// </summary>
public struct JsonDataValue
{
    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// JSON for field and value
/// </summary>
public struct JsonDataFieldValue
{
    /// <summary>
    /// Field
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// JSON for field, old and new values
/// </summary>
public struct JsonDataFieldOldNew
{
    /// <summary>
    /// Field
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Old value
    /// </summary>
    public string Old { get; set; }

    /// <summary>
    /// New value
    /// </summary>
    public string New { get; set; }
}

/// <summary>
/// JSON for field, operation and value
/// </summary>
public struct JsonDataFieldOperationValue
{
    /// <summary>
    /// Field
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Operation
    /// </summary>
    public string Operation { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }
}
